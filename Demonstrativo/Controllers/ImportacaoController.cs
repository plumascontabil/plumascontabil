using CsvHelper;
using Demonstrativo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OFXParser.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Controllers
{
    public class ImportacaoController : Controller
    {
        Context _context;
        IHostingEnvironment _appEnvironment;

        public ImportacaoController(Context context, IHostingEnvironment env)
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
        public async Task<IActionResult> ImportarOfx(IFormFile fileOfx)
        {
            List<Empresa> empresas = _context.Empresas.ToList();

            var historicos = _context.HistoricosOfx.ToList();
            string caminhoDestinoArquivo = $"{_appEnvironment.WebRootPath}\\Temp\\{ fileOfx.FileName}";
            using (var stream = new FileStream(caminhoDestinoArquivo, FileMode.Create))
            {
                await fileOfx.CopyToAsync(stream);
            }
            Extract extratoBancario = OFXParser.Parser.GenerateExtract(caminhoDestinoArquivo);

            var importacaoOfxViewModel = new ImportacaoOfxViewModel();
            var dadosViewModel = new List<DadosViewModel>();
            var historicoOfxViewModel = new List<HistoricoOfxViewModel>();

            var contasContabeis = _context.ContasContabeis.ToList();

            foreach (var record in extratoBancario.Transactions)
            {
                var historico = historicos.FirstOrDefault(h => h.Descricao == record.Description);

                if (historico == null)
                {
                    var empresasSelectList = ConstruirEmpresas(empresas);
                    var contasContabeisSelectList = ConstruirContasContabeisSelectList(contasContabeis);

                    dadosViewModel.Add(new DadosViewModel()
                    {
                        Id = record.Id,
                        TransationValue = record.TransactionValue,
                        Description = record.Description,
                        Date = record.Date,
                        CheckSum = record.Checksum,
                        Type = record.Type,
                        ContasCreditoContabeis = contasContabeisSelectList,
                        ContasDebitoContabeis = contasContabeisSelectList,
                    }) ;
                    importacaoOfxViewModel.Dados = dadosViewModel; //Add(new ImportacaoOfxViewModel() { Dados = dadosViewModel });
                }
                else
                {
                    dadosViewModel.Add(new DadosViewModel()
                    {
                        Id = record.Id,
                        TransationValue = record.TransactionValue,
                        Description = record.Description,
                        Date = record.Date,
                        CheckSum = record.Checksum,
                        Type = record.Type,
                        ContasCreditoSelecionada = historico.ContaCreditoId,
                        ContasDebitoSelecionada = historico.ContaDebitoId,
                        HistoricoId = historico.Id,
                        ContasCreditoContabeis = ConstruirContasContabeisSelectList(contasContabeis.Where(x => x.Codigo == historico.ContaCreditoId)),                        
                        ContasDebitoContabeis = ConstruirContasContabeisSelectList(contasContabeis.Where(x => x.Codigo == historico.ContaDebitoId)),
                    });
                    importacaoOfxViewModel.Dados = dadosViewModel ;
                }
                importacaoOfxViewModel.Empresas = ConstruirEmpresas(empresas);
            }
            System.IO.File.Delete(caminhoDestinoArquivo);
            System.IO.File.Delete($"{caminhoDestinoArquivo}.xml");

            return View("Ofx", importacaoOfxViewModel);
        }

        public IActionResult ViewHistorico()
        {
            return View("Historico");
        }
        
        [HttpPost]
        public IActionResult GravarOfx(ImportacaoOfxViewModel ofxs)
        {
            foreach (var ofx in ofxs.Dados)
            {
                var desc = _context.HistoricosOfx.Any(h => h.Descricao == ofx.Description);
                
                if(desc == false)
                {
                    //Todo: adicionar histórico
                    _context.HistoricosOfx.Add(new HistoricoOfx()
                    {
                        Descricao = ofx.Description,
                        ContaCreditoId = ofx.ContasCreditoSelecionada,
                        ContaDebitoId = ofx.ContasDebitoSelecionada
                    });
                    _context.SaveChanges();
                }
                var historico = _context.HistoricosOfx.FirstOrDefault(h => h.Descricao == ofx.Description);
                _context.Ofxs.Add(new ImportacaoOfx()
                {
                    Documento = ofx.Id,
                    TipoLancamento = ofx.Type,
                    Descricao = ofx.Description,
                    ValorOfx = ofx.TransationValue,
                    Data = ofx.Date,
                    HistoricoOfxId = historico.Id,
                    EmpresaId = ofxs.EmpresaSelecionada,
                });
            }

            _context.SaveChanges();

            return View("Index");
        }

        [HttpPost]
        public IActionResult GravarHistoricoOfx(HistoricoOfxViewModel historicoOfx)
        {
            var historico = new HistoricoOfx()
            {
                Descricao = historicoOfx.Descricao,
                ContaDebitoId = historicoOfx.CodigoContaDebitoSelecionada,
                ContaCreditoId = historicoOfx.CodigoContaCreditoSelecionada,
            };

            _context.HistoricosOfx.Add(historico);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private static SelectList ConstruirContasContabeisSelectList(IEnumerable<ContaContabil> contasContabeis)        
            => new(contasContabeis.Select(c => new { c.Codigo, Descricao = $"{c.Codigo} - {c.Historico}" }), "Codigo", "Descricao");

        private static SelectList ConstruirEmpresas(IEnumerable<Empresa> empresas)
            => new(empresas.Select(e => new { e.Codigo, Razao = $"{e.Codigo} - {e.RazaoSocial}" }), "Codigo", "Razao");

    }
}
