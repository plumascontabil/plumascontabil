using CsvHelper;
using Demonstrativo.Models;
using DomainService;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Demonstrativo.Controllers
{
    public class ImportacaoController : BaseController
    {
        readonly static BaseFont fonteBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
        readonly Context _context;
        readonly IWebHostEnvironment _appEnvironment;
        private readonly ILogger _logger;
        //private readonly ImportacaoDomainService _importacaoDomainService;

        public ImportacaoController(
            Context context,
            IWebHostEnvironment env,
            ILogger<AutoDescricao> logger,
            //ImportacaoDomainService importacaoDomainService
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager) : base(context, roleManager)
        {
            _context = context;
            _appEnvironment = env;
            _logger = logger;
            //_importacaoDomainService = importacaoDomainService;
        }
        public IActionResult Index()
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Importar(IFormFile file)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            var stream = file.OpenReadStream();
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, new CultureInfo("pt-BR")))
            {
                var records = csv.GetRecords<ImportacaoCsvViewModel>().ToList();

                foreach (var record in records)
                {
                    var insertEmpresa = new Empresa()
                    {
                        Codigo = record.Codigo,
                        RazaoSocial = record.Razao,
                        Apelido = record.Apelido,
                        Cnpj = record.Cnpj ?? "00.000.000/0001-00",
                        Situacao = record.Situacao,
                    };

                    _context.Empresas.Add(insertEmpresa);
                    _context.SaveChanges();
                }
            }
            _logger.LogInformation(((int)EEventLog.Post), "Import created.");

            //await _importacaoDomainService.Importar(file);
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ImportarContasContabeis(IFormFile fileContasContabeis)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            var stream = fileContasContabeis.OpenReadStream();

            using (var reader = new StreamReader(stream))
            using (var csvContas = new CsvReader(reader, new CultureInfo("pt-BR")))
            {
                var records = csvContas.GetRecords<ImportacaoContaContabilViewModel>().ToList();

                foreach (var record in records)
                {
                    var insertContaContabil = new ContaContabil()
                    {
                        Codigo = record.Codigo,
                        Historico = record.Descricao,
                    };

                    _context.ContasContabeis.Add(insertContaContabil);
                }
                _context.SaveChanges();
            }
            _logger.LogInformation(((int)EEventLog.Post), "Import Conta Contabil created.");
            //await _importacaoDomainService.ImportarContasContabeis(file);
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View("Index");
        }




        [HttpPost]
        public IActionResult Filtrar(RelatorioViewModel relatorioViewModel)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            var contaContabil = _context.ContasContabeis.FirstOrDefault(c => c.Codigo == relatorioViewModel.ContaContabil);
            //var historicos = _context.OfxDescricoes.Where(h => h.ContaDebitoId == contaContabil.Codigo
            //                                    || h.ContaCreditoId == contaContabil.Codigo);
            var dadosOfx = _context.OfxLancamentos.Where(o => (o.Data.Date >= relatorioViewModel.DataInicial.Date
                                                && o.Data.Date <= relatorioViewModel.DataFinal.Date));
            var contasCorrentes = _context.ContasCorrentes.Where(c => c.EmpresaId == relatorioViewModel.Empresa);
            var relatorioDadosViewModel = new List<RelatorioViewModel>();

            foreach (var contaCorrente in contasCorrentes)
            {
                foreach (var dado in dadosOfx)
                {
                    relatorioDadosViewModel.Add(new RelatorioViewModel
                    {
                        RazaoEmpresa = contaCorrente.EmpresaId,
                        Date = dado.Data,
                        Type = dado.TipoLancamento,
                        Description = dado.Descricao,
                        TransationValue = dado.ValorOfx,
                    });
                }
            }
            //var relatorioDadosViewModel = await _importacaoDomainService.Filtrar(relatorioViewModel);
            _logger.LogInformation(((int)EEventLog.Post), "filtered report displayed.");
            GerarRelatorioRazao(relatorioViewModel);
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View("RelatorioExibir", relatorioDadosViewModel);
        }

        private static SelectList ConstruirContasContabeisSelectList(IEnumerable<ContaContabil> contasContabeis)
            => new(contasContabeis.Select(c => new { c.Codigo, Descricao = $"{c.Codigo} - {c.Historico}" }), "Codigo", "Descricao");
        private static SelectList ConstruirEmpresas(IEnumerable<Empresa> empresas)
            => new(empresas.Select(e => new { e.Codigo, Razao = $"{e.Codigo} - {e.RazaoSocial}" }), "Codigo", "Razao");


        public async Task<IActionResult> RelatorioOfx()
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            var empresas = _context.Empresas.ToList();
            var contasContabeis = _context.ContasContabeis.ToList();
            var relatorioViewModel = new RelatorioViewModel()
            {
                Empresas = ConstruirEmpresas(empresas),
                ContasContabeis = ConstruirContasContabeisSelectList(contasContabeis),
            };
            //var relatorioViewModel = await _importacaoDomainService.RelatorioOfx();
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View("RelatorioOfx", relatorioViewModel);
        }
        public void GerarRelatorioRazao(RelatorioViewModel relatorioViewModel)
        {
            var contasContabeis = _context.ContasContabeis.ToList();

            var contaContabil = _context.ContasContabeis.FirstOrDefault(c => c.Codigo == relatorioViewModel.ContaContabil);

            //var descricoes = _context.OfxDescricoes.Where(h => h.ContaDebitoId == contaContabil.Codigo
            //                                    || h.ContaCreditoId == contaContabil.Codigo);

            var dadosOfx = _context.OfxLancamentos.Where(o => (o.Data.Date >= relatorioViewModel.DataInicial.Date
                                                && o.Data.Date <= relatorioViewModel.DataFinal.Date));

            var contasCorrentes = _context.ContasCorrentes.Where(c => c.EmpresaId == relatorioViewModel.Empresa);

            var empresas = _context.Empresas.ToList();

            //var complementos = _context.OfxComplementos.ToList();

            var relatorioDadosViewModel = new List<RelatorioViewModel>();

            //config doc pdf
            var pxParaMm = 72 / 25.2F;
            var pdf = new Document(PageSize.A4, 15 * pxParaMm, 15 * pxParaMm, 15 * pxParaMm, 20 * pxParaMm);
            var caminhoPDF = $"{_appEnvironment.WebRootPath}\\Temp\\Razao.{DateTime.Now:yyyy.MM.dd.HH.mm.ss}.pdf";
            var nomeArquivo = $"";
            var arquivo = new FileStream(caminhoPDF, FileMode.Create);
            var writer = PdfWriter.GetInstance(pdf, arquivo);
            pdf.Open();
            //titulo
            var fonteParagrafo = new iTextSharp.text.Font(fonteBase, 32,
                iTextSharp.text.Font.NORMAL, BaseColor.Black);
            var titulo = new Paragraph("Relatório Razão\n\n", fonteParagrafo)
            {
                Alignment = Element.ALIGN_LEFT
            };
            pdf.Add(titulo);

            //add imagem
            var caminhoImagem = "https://plumascontabil.com.br/wp-content/uploads/2019/06/LOGO_AJUSTADO_NOVO.png";
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(caminhoImagem);
            if (logo != null)
            {
                float razaoAlturaLargura = logo.Width / logo.Height;
                float alturaLogo = 32;
                float larguraLogo = alturaLogo * razaoAlturaLargura;
                logo.ScaleToFit(larguraLogo, alturaLogo);
                var margemEsquerda = pdf.PageSize.Width - pdf.TopMargin - larguraLogo;
                var margemTopo = pdf.PageSize.Height - pdf.TopMargin - 54;
                logo.SetAbsolutePosition(margemEsquerda, margemTopo);
                writer.DirectContent.AddImage(logo, false);
            }

            //Adicionando tabela
            var tabela = new PdfPTable(8);
            float[] largurasColunas = { 0.6f, 2f, 1.5f, 1.5f, 0.6f, 0.6f, 1f, 1f };
            tabela.DefaultCell.BorderWidth = 0;
            tabela.WidthPercentage = 100;

            //Adicionando Celula/Coluna
            CriarCelulaTexto(tabela, "Data", PdfCell.ALIGN_CENTER, true);
            CriarCelulaTexto(tabela, "Empresa", PdfCell.ALIGN_CENTER, true);
            CriarCelulaTexto(tabela, "Descrição", PdfCell.ALIGN_CENTER, true);
            CriarCelulaTexto(tabela, "Complemento", PdfCell.ALIGN_CENTER, true);
            CriarCelulaTexto(tabela, "Valor", PdfCell.ALIGN_CENTER, true);
            CriarCelulaTexto(tabela, "Tipo", PdfCell.ALIGN_CENTER, true);
            CriarCelulaTexto(tabela, "ContaCredito", PdfCell.ALIGN_CENTER, true);
            CriarCelulaTexto(tabela, "ContaDebito", PdfCell.ALIGN_CENTER, true);

            foreach (var contaCorrente in contasCorrentes.Where(c => c.EmpresaId == relatorioViewModel.Empresa))
            {
                //foreach (var dado in dadosOfx.Where(c => c.ContaCorrenteId == contaCorrente.Id))
                //{
                //    var complemento = complementos.FirstOrDefault(c => c.OfxId == dado.Id);
                //    if(complemento != null)
                //    {
                //        var descricao = descricoes.FirstOrDefault(c => c.Id == complemento.HistoricoId);
                //        if (descricao != null)
                //        {
                //            //Conteudo
                //            CriarCelulaTexto(tabela, dado.Data.ToString("dd/MM/yyyy"), PdfCell.ALIGN_CENTER);
                //            CriarCelulaTexto(tabela, empresas.FirstOrDefault(
                //                e => e.Codigo == contaCorrente.EmpresaId).RazaoSocial, PdfCell.ALIGN_CENTER);
                //            CriarCelulaTexto(tabela, dado.Descricao, PdfCell.ALIGN_CENTER);
                //            CriarCelulaTexto(tabela, complemento.DescricaoComplemento, PdfCell.ALIGN_CENTER);
                //            CriarCelulaTexto(tabela, dado.ValorOfx.ToString(), PdfCell.ALIGN_CENTER);
                //            CriarCelulaTexto(tabela, dado.TipoLancamento, PdfCell.ALIGN_CENTER);
                //            CriarCelulaTexto(tabela, contasContabeis.FirstOrDefault(c => c.Codigo == descricao.ContaCreditoId).Historico, PdfCell.ALIGN_CENTER);
                //            CriarCelulaTexto(tabela, contasContabeis.FirstOrDefault(c => c.Codigo == descricao.ContaDebitoId).Historico, PdfCell.ALIGN_CENTER);
                //            //CriarCelulaTexto(tabela, descricao.ContaCreditoId == relatorioViewModel.ContaContabil
                //            //        ? contaContabil.Historico
                //            //        : contaContabil.Historico,
                //            //        PdfCell.ALIGN_CENTER);
                //        }
                //    }
                //}
            }

            pdf.Add(tabela);

            pdf.Close();
            arquivo.Close();

            //abrir arquivo no visualizador padrão
            if (System.IO.File.Exists(caminhoPDF))
            {
                Process.Start(new ProcessStartInfo()
                {
                    Arguments = $"/c start {caminhoPDF}",
                    FileName = "cmd.exe",
                    CreateNoWindow = true
                });
            }
        }
        private static void CriarCelulaTexto(PdfPTable tabela, string texto,
            int alinhamentoHorz = PdfPCell.ALIGN_LEFT,
            bool negrito = false, bool italico = false,
            int tamanhoFonte = 9, int alturaCelula = 25)
        {
            int estilo = iTextSharp.text.Font.NORMAL;
            if (negrito && italico)
            {
                estilo = iTextSharp.text.Font.BOLDITALIC;
            }
            else if (negrito)
            {
                estilo = iTextSharp.text.Font.BOLD;
            }
            else if (italico)
            {
                estilo = iTextSharp.text.Font.ITALIC;
            }
            var fonteCelula = new iTextSharp.text.Font(fonteBase, tamanhoFonte,
                estilo, BaseColor.Black);
            var celula = new PdfPCell(new Phrase(texto, fonteCelula))
            {
                HorizontalAlignment = alinhamentoHorz,
                VerticalAlignment = PdfCell.ALIGN_MIDDLE,
                Border = 0,
                BorderWidthBottom = 1,
                FixedHeight = alturaCelula
            };
            tabela.AddCell(celula);
        }
    }
}