using CsvHelper;
using Demonstrativo.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OFXParser.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Controllers
{
    public class ImportacaoController : Controller
    {
        static BaseFont fonteBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
        Context _context;
        IWebHostEnvironment _appEnvironment;
        public ImportacaoController(Context context, IWebHostEnvironment env)
        {
            _context = context;
            _appEnvironment = env;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Importar(IFormFile file)
        {
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
            return View("Index");
        }
        [HttpPost]
        public IActionResult ImportarContasContabeis(IFormFile fileContasContabeis)
        {
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
            return View("Index");
        }




        [HttpPost]
        public async Task<IActionResult> ImportarOfx(IFormFile fileOfx = null, ExtratoBancarioViewModel extratoView = null)
        {
            List<Empresa> empresas = _context.Empresas.ToList();

            var historicos = _context.OfxDescricoes.ToList();

            var lancamentoOfxViewModel = new List<OfxLancamentoViewModel>();
            var contaCorrenteViewModel = new OfxContaCorrenteViewModel();
            var historicoOfxViewModel = new List<OfxDescricaoViewModel>();
            var extratoBancarioViewModel = new ExtratoBancarioViewModel();

            var contasContabeis = _context.ContasContabeis.ToList();

            if(fileOfx != null)
            {
                string caminhoDestinoArquivo = $"{_appEnvironment.WebRootPath}\\Temp\\{ fileOfx.FileName}";
                using (var stream = new FileStream(caminhoDestinoArquivo, FileMode.Create))
                {
                    await fileOfx.CopyToAsync(stream);
                }
                Extract extratoBancario = OFXParser.Parser.GenerateExtract(caminhoDestinoArquivo);

                foreach (var dados in extratoBancario.Transactions)
                {
                    var empresasSelectList = ConstruirEmpresas(empresas);
                    var contasContabeisSelectList = ConstruirContasContabeisSelectList(contasContabeis);

                    lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
                    {
                        Id = dados.Id,
                        TransationValue = dados.TransactionValue,
                        Description = dados.Description,
                        Date = dados.Date,
                        CheckSum = dados.Checksum,
                        Type = dados.Type,
                        ContasCredito = contasContabeisSelectList,
                        ContasDebito = contasContabeisSelectList,
                    });

                    var banco = _context.OfxBancos.FirstOrDefault(b => b.Codigo == extratoBancario.BankAccount.Bank.Code);

                    var bancoViewModel = new OfxBancoViewModel()
                    {
                        Id = banco.Id,
                        Codigo = banco.Codigo,
                        Nome = banco.Nome
                    };

                    contaCorrenteViewModel = new OfxContaCorrenteViewModel()
                    {
                        LancamentosOfxs = lancamentoOfxViewModel,
                        NumeroConta = extratoBancario.BankAccount.AccountCode
                    };

                    var complementoViewModel = new OfxComplementoViewModel()
                    { 
                        LancamentoOfx = lancamentoOfxViewModel,
                    };

                    extratoBancarioViewModel = new ExtratoBancarioViewModel()
                    {
                        ComplementoOfxViewModel = complementoViewModel,
                        Empresas = empresasSelectList,
                        ContasCorrentes = contaCorrenteViewModel,
                        Banco = bancoViewModel
                    };
                }

                System.IO.File.Delete(caminhoDestinoArquivo);
                System.IO.File.Delete($"{caminhoDestinoArquivo}.xml");
            }
            else
            {
                var empresasSelectList = ConstruirEmpresas(empresas);
                var contasContabeisSelectList = ConstruirContasContabeisSelectList(contasContabeis);
                foreach (var dados in extratoView.ContasCorrentes.LancamentosOfxs)
                {
                    var historico = historicos.FirstOrDefault(h => h.Descricao == extratoView.LancamentoManual.Descricao);

                    if (historico == null)
                    {

                        lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
                        {
                            Id = dados.Id,
                            TransationValue = dados.TransationValue,
                            Description = dados.Description,
                            Date = dados.Date,
                            CheckSum = dados.CheckSum,
                            Type = dados.Type,
                            ContasCredito = contasContabeisSelectList,
                            ContasDebito = contasContabeisSelectList,
                        });
                    }
                    else
                    {
                        lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
                        {
                            TransationValue = extratoView.LancamentoManual.Valor,
                            Description = extratoView.LancamentoManual.Descricao,
                            Date = extratoView.LancamentoManual.Data,
                            CheckSum = 1,
                            Type = extratoView.LancamentoManual.TipoSelecionado,
                            ContaCreditoSelecionada = historico.ContaCreditoId,
                            ContaDebitoSelecionada = historico.ContaDebitoId,
                            HistoricoId = historico.Id,
                            ContasCredito = ConstruirContasContabeisSelectList(contasContabeis.Where(x => x.Codigo == historico.ContaCreditoId)),
                            ContasDebito = ConstruirContasContabeisSelectList(contasContabeis.Where(x => x.Codigo == historico.ContaDebitoId)),
                        });
                    }

                    var banco = _context.OfxBancos.FirstOrDefault(b => b.Codigo == extratoView.Banco.Codigo);

                    var bancoViewModel = new OfxBancoViewModel()
                    {
                        Id = banco.Id,
                        Codigo = banco.Codigo,
                        Nome = banco.Nome
                    };

                    contaCorrenteViewModel = new OfxContaCorrenteViewModel()
                    {
                        LancamentosOfxs = lancamentoOfxViewModel,
                        NumeroConta = extratoView.ContasCorrentes.NumeroConta
                    };
                    extratoBancarioViewModel = new ExtratoBancarioViewModel()
                    {
                        Empresas = ConstruirEmpresas(empresas),
                        ContasCorrentes = contaCorrenteViewModel,
                        Banco = bancoViewModel
                    };
                }

                if (extratoView.LancamentoManual != null)
                {
                    lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
                    {
                        TransationValue = extratoView.LancamentoManual.Valor,
                        Description = extratoView.LancamentoManual.Descricao,
                        Date = extratoView.LancamentoManual.Data,
                        CheckSum = 1,
                        Type = extratoView.LancamentoManual.TipoSelecionado,
                        ContasCredito = contasContabeisSelectList,
                        ContasDebito = contasContabeisSelectList,
                    });
                }
            }

            return View("Ofx", extratoBancarioViewModel);
        }
        [HttpPost]
        public IActionResult Filtrar(RelatorioViewModel relatorioViewModel)
        {
            var contaContabil = _context.ContasContabeis.FirstOrDefault(c => c.Codigo == relatorioViewModel.ContaContabil);
            var historicos = _context.OfxDescricoes.Where(h => h.ContaDebitoId == contaContabil.Codigo
                                                || h.ContaCreditoId == contaContabil.Codigo);
            var dadosOfx = _context.OfxLancamentos.Where(o => (o.Data.Date >= relatorioViewModel.DataInicial.Date
                                                && o.Data.Date <= relatorioViewModel.DataFinal.Date));
            var contasCorrentes = _context.ConstasCorrentes.Where(c => c.EmpresaId == relatorioViewModel.Empresa);
            var relatorioDadosViewModel = new List<RelatorioViewModel>();

            foreach (var historico in historicos)
            {
                foreach (var contaCorrente in contasCorrentes)
                {
                    foreach(var dado in dadosOfx)
                    {
                        relatorioDadosViewModel.Add(new RelatorioViewModel 
                        {
                            RazaoEmpresa = contaCorrente.EmpresaId,
                            Date = dado.Data,
                            Type = dado.TipoLancamento,
                            Description = dado.Descricao,
                            TransationValue = dado.ValorOfx,
                            ContaDebitar = historico.ContaDebitoId,
                            ContaCreditar = historico.ContaCreditoId
                        });
                    }
                }
            }

            GerarRelatorioRazao(relatorioViewModel);
            
            return View("RelatorioExibir", relatorioDadosViewModel);
        }
        [HttpPost]
        public IActionResult GravarOfx(ExtratoBancarioViewModel ofxs)
        {
            foreach (var ofx in ofxs.ContasCorrentes.LancamentosOfxs)
            {
                var cc = _context.ConstasCorrentes.Any(c => c.NumeroConta == ofxs.ContasCorrentes.NumeroConta);
                var banco = _context.OfxBancos.Any(b => b.Codigo == ofxs.Banco.Codigo);

                if (banco == false)
                {
                    //banco não cadastrado
                }

                if (cc == false)
                {
                    //Todo: adicionar Conta corrente
                    _context.ConstasCorrentes.Add(new OfxContaCorrente()
                    {
                        NumeroConta = ofxs.ContasCorrentes.NumeroConta,
                        EmpresaId = _context.Empresas.FirstOrDefault(e => e.Codigo == ofxs.EmpresaSelecionada).Codigo,
                        BancoOfxId = _context.OfxBancos.FirstOrDefault(b => b.Codigo == ofxs.Banco.Codigo).Id,
                    });
                    _context.SaveChanges();
                }

                //Todo: adicionar descricao do historico
                _context.OfxDescricoes.Add(new OfxDescricao()
                {
                    Descricao = ofx.Description,
                    ContaCreditoId = ofx.ContaCreditoSelecionada,
                    ContaDebitoId = ofx.ContaDebitoSelecionada
                });
                _context.SaveChanges();

                _context.OfxLancamentos.Add(new OfxLancamento()
                {
                    Documento = ofx.Id,
                    TipoLancamento = ofx.Type,
                    Descricao = ofx.Description,
                    ValorOfx = ofx.TransationValue,
                    Data = ofx.Date,
                    ContaCorrenteId = _context.ConstasCorrentes.FirstOrDefault(c => c.NumeroConta == ofxs.ContasCorrentes.NumeroConta).Id
                    
                });
                _context.SaveChanges();
                _context.OfxComplementos.Add(new OfxComplemento()
                {
                    DescricaoComplemento = ofx.Complemento,
                    HistoricoId = _context.OfxDescricoes.OrderBy(d => d.Id).Last().Id,
                    OfxId = _context.OfxLancamentos.OrderBy(l => l.Id).Last().Id,
                });
            }

            _context.SaveChanges();

            return View("Index");
        }
        private static SelectList ConstruirContasContabeisSelectList(IEnumerable<ContaContabil> contasContabeis)        
            => new(contasContabeis.Select(c => new { c.Codigo, Descricao = $"{c.Codigo} - {c.Historico}" }), "Codigo", "Descricao");
        private static SelectList ConstruirEmpresas(IEnumerable<Empresa> empresas)
            => new(empresas.Select(e => new { e.Codigo, Razao = $"{e.Codigo} - {e.RazaoSocial}" }), "Codigo", "Razao");
        private static SelectList ContruirBancos(IEnumerable<OfxBanco> bancos)
            => new(bancos.Select(c => new { c.Codigo, Nome = $"{c.Codigo} - {c.Nome}" }), "Codigo", "Nome");
        public IActionResult RelatorioOfx()
        {
            var empresas = _context.Empresas.ToList();
            var contasContabeis = _context.ContasContabeis.ToList();
            var relatorioViewModel = new RelatorioViewModel()
            {
                Empresas = ConstruirEmpresas(empresas),
                ContasContabeis = ConstruirContasContabeisSelectList(contasContabeis),
            };

            return View("RelatorioOfx", relatorioViewModel);
        }
        public void GerarRelatorioRazao(RelatorioViewModel relatorioViewModel)
        {
            var contasContabeis = _context.ContasContabeis.ToList();

            var contaContabil = _context.ContasContabeis.FirstOrDefault(c => c.Codigo == relatorioViewModel.ContaContabil);

            var descricoes = _context.OfxDescricoes.Where(h => h.ContaDebitoId == contaContabil.Codigo
                                                || h.ContaCreditoId == contaContabil.Codigo);

            var dadosOfx = _context.OfxLancamentos.Where(o => (o.Data.Date >= relatorioViewModel.DataInicial.Date
                                                && o.Data.Date <= relatorioViewModel.DataFinal.Date));

            var contasCorrentes = _context.ConstasCorrentes.Where(c => c.EmpresaId == relatorioViewModel.Empresa);

            var empresas = _context.Empresas.ToList();

            var complementos = _context.OfxComplementos.ToList();

            var relatorioDadosViewModel = new List<RelatorioViewModel>();

            //config doc pdf
            var pxParaMm = 72 / 25.2F;
            var pdf = new Document(PageSize.A4, 15 * pxParaMm, 15 * pxParaMm, 15 * pxParaMm, 20 * pxParaMm);
            var caminhoPDF = $"{_appEnvironment.WebRootPath}\\Temp\\Razao.{DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss")}.pdf";
            var nomeArquivo = $"";
            var arquivo = new FileStream(caminhoPDF, FileMode.Create);
            var writer = PdfWriter.GetInstance(pdf, arquivo);
            pdf.Open();
            //titulo
            var fonteParagrafo = new iTextSharp.text.Font(fonteBase, 32,
                iTextSharp.text.Font.NORMAL, BaseColor.Black);
            var titulo = new Paragraph("Relatório Razão\n\n", fonteParagrafo);
            titulo.Alignment = Element.ALIGN_LEFT;
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
                foreach (var dado in dadosOfx.Where(c => c.ContaCorrenteId == contaCorrente.Id))
                {
                    var complemento = complementos.FirstOrDefault(c => c.OfxId == dado.Id);
                    if(complemento != null)
                    {
                        var descricao = descricoes.FirstOrDefault(c => c.Id == complemento.HistoricoId);
                        if (descricao != null)
                        {
                            //Conteudo
                            CriarCelulaTexto(tabela, dado.Data.ToString("dd/MM/yyyy"), PdfCell.ALIGN_CENTER);
                            CriarCelulaTexto(tabela, empresas.FirstOrDefault(
                                e => e.Codigo == contaCorrente.EmpresaId).RazaoSocial, PdfCell.ALIGN_CENTER);
                            CriarCelulaTexto(tabela, dado.Descricao, PdfCell.ALIGN_CENTER);
                            CriarCelulaTexto(tabela, complemento.DescricaoComplemento, PdfCell.ALIGN_CENTER);
                            CriarCelulaTexto(tabela, dado.ValorOfx.ToString(), PdfCell.ALIGN_CENTER);
                            CriarCelulaTexto(tabela, dado.TipoLancamento, PdfCell.ALIGN_CENTER);
                            CriarCelulaTexto(tabela, contasContabeis.FirstOrDefault(c => c.Codigo == descricao.ContaCreditoId).Historico, PdfCell.ALIGN_CENTER);
                            CriarCelulaTexto(tabela, contasContabeis.FirstOrDefault(c => c.Codigo == descricao.ContaDebitoId).Historico, PdfCell.ALIGN_CENTER);
                            //CriarCelulaTexto(tabela, descricao.ContaCreditoId == relatorioViewModel.ContaContabil
                            //        ? contaContabil.Historico
                            //        : contaContabil.Historico,
                            //        PdfCell.ALIGN_CENTER);
                        }
                    }
                }
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
            if(negrito && italico)
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
            var celula = new PdfPCell(new Phrase(texto, fonteCelula));
            celula.HorizontalAlignment = alinhamentoHorz;
            celula.VerticalAlignment = PdfCell.ALIGN_MIDDLE;
            celula.Border = 0;
            celula.BorderWidthBottom = 1;
            celula.FixedHeight = alturaCelula;
            tabela.AddCell(celula);
        }
    }
}