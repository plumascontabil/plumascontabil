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
                var records = csv.GetRecords<ImportacaoViewModel>().ToList();
                
                foreach (var record in records)
                {
                    var insertEmpresa = new Empresa();

                    insertEmpresa.Codigo = record.Codigo;
                    insertEmpresa.RazaoSocial = record.Razao;
                    insertEmpresa.Apelido = record.Apelido;
                    insertEmpresa.Cnpj = record.Cnpj ?? "00.000.000/0001-00";
                    insertEmpresa.Situacao = record.Situacao;

                    _context.Empresas.Add(insertEmpresa);
                    _context.SaveChanges();
                }
            }
            return View("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> ImportarOfx(IFormFile fileOfx)
        {
            var historicos = _context.HistoricosOfx.ToList();
            string caminhoDestinoArquivo = $"{_appEnvironment.WebRootPath}\\Temp\\{ fileOfx.FileName}";
            using (var stream = new FileStream(caminhoDestinoArquivo, FileMode.Create))
            {
                await fileOfx.CopyToAsync(stream);
            }
            Extract extratoBancario = OFXParser.Parser.GenerateExtract(caminhoDestinoArquivo);

            var importacaoOfxViewModel = new List<ImportacaoOfxViewModel>();
            var historicoOfxViewModel = new List<HistoricoOfxViewModel>();

            var contasContabeis = _context.ContasContabeis.ToList();

            foreach (var record in extratoBancario.Transactions)
            {
                
                var historico = historicos.FirstOrDefault(h => h.Descricao == record.Description);

                if (historico == null)
                {

                    var contasContabeisSelectList = ConstruirContasContabeisSelectList(contasContabeis);

                    importacaoOfxViewModel.Add(new ImportacaoOfxViewModel()
                    {
                        Id = record.Id,
                        TransationValue = record.TransactionValue,
                        Description = record.Description,
                        Date = record.Date,
                        CheckSum = record.Checksum,
                        Type = record.Type,
                        ContasCreditoContabeis = contasContabeisSelectList,
                        ContasDebitoContabeis = contasContabeisSelectList
                    }) ;
                }
                else
                {

                    importacaoOfxViewModel.Add(new ImportacaoOfxViewModel()
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
                    }) ;
                }
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
        public IActionResult GravarOfx(List<ImportacaoOfxViewModel> ofxs)
        {
            foreach (var ofx in ofxs)
            {
                if(ofx.HistoricoId == null)
                {
                    //Todo: adicionar histórico
                    _context.HistoricosOfx.Add(new HistoricoOfx()
                    {
                        Descricao = ofx.Description,
                        ContaCreditoId = ofx.ContasCreditoSelecionada,
                        ContaDebitoId = ofx.ContasDebitoSelecionada
                    });
                    _context.SaveChanges();

                    ofx.HistoricoId = 3434;
                }

                _context.Ofxs.Add(new ImportacaoOfx()
                {
                    Data = ofx.Date
                });
            }

            _context.SaveChanges();

            return View("Index");
        }

        [HttpPost]
        public IActionResult GravarHistoricoOfx(HistoricoOfxViewModel historicoOfx)
        {
            var historico = new HistoricoOfx();

            historico.Descricao = historicoOfx.Descricao;
            historico.ContaDebitoId = historicoOfx.CodigoContaDebitoSelecionada;
            historico.ContaCreditoId = historicoOfx.CodigoContaCreditoSelecionada;

            _context.HistoricosOfx.Add(historico);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        private static SelectList ConstruirContasContabeisSelectList(IEnumerable<ContaContabil> contasContabeis)        
            => new(contasContabeis.Select(c => new { c.Codigo, Descricao = $"{c.Codigo} - {c.Historico}" }), "Codigo", "Descricao");
         
    }
}
