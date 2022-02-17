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

            var lancamentoOfxViewModel = new List<LancamentoOfxViewModel>();
            var contaCorrenteViewModel = new ContaCorrenteViewModel();
            var historicoOfxViewModel = new List<HistoricoOfxViewModel>();
            var extratoBancarioViewModel = new ExtratoBancarioViewModel();

            var contasContabeis = _context.ContasContabeis.ToList();
            foreach (var dados in extratoBancario.Transactions)
            {
                var historico = historicos.FirstOrDefault(h => h.Descricao == dados.Description);

                if (historico == null)
                {
                    var empresasSelectList = ConstruirEmpresas(empresas);
                    var contasContabeisSelectList = ConstruirContasContabeisSelectList(contasContabeis);

                    lancamentoOfxViewModel.Add(new LancamentoOfxViewModel()
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
                }
                else
                {
                        lancamentoOfxViewModel.Add(new LancamentoOfxViewModel()
                        {
                            Id = dados.Id,
                            TransationValue = dados.TransactionValue,
                            Description = dados.Description,
                            Date = dados.Date,
                            CheckSum = dados.Checksum,
                            Type = dados.Type,
                            ContaCreditoSelecionada = historico.ContaCreditoId,
                            ContaDebitoSelecionada = historico.ContaDebitoId,
                            HistoricoId = historico.Id,
                            ContasCredito = ConstruirContasContabeisSelectList(contasContabeis.Where(x => x.Codigo == historico.ContaCreditoId)),                        
                            ContasDebito = ConstruirContasContabeisSelectList(contasContabeis.Where(x => x.Codigo == historico.ContaDebitoId)),
                        });
                }

                var banco = _context.BancoOfxs.FirstOrDefault(b => b.Codigo == extratoBancario.BankAccount.Bank.Code);

                var bancoViewModel = new BancoViewModel()
                {
                    Id = banco.Id,
                    Codigo = banco.Codigo,
                    Nome = banco.Nome
                };

                contaCorrenteViewModel = new ContaCorrenteViewModel()
                {
                    LancamentosOfxs = lancamentoOfxViewModel,
                    NumeroConta = extratoBancario.BankAccount.AccountCode
                };
                extratoBancarioViewModel = new ExtratoBancarioViewModel()
                { 
                    Empresas = ConstruirEmpresas(empresas), 
                    ContasCorrentes = contaCorrenteViewModel,
                    Banco = bancoViewModel
                };
            }

            System.IO.File.Delete(caminhoDestinoArquivo);
            System.IO.File.Delete($"{caminhoDestinoArquivo}.xml");

            return View("Ofx", extratoBancarioViewModel);
        }

        public IActionResult ContaCorrente()
        {
            var empresas = _context.Empresas.ToList();
            var bancos = _context.BancoOfxs.ToList();

            var contaCorrente = new ContaCorrenteViewModel()
            {
                Empresas = ConstruirEmpresas(empresas),
                Bancos = ContruirBancos(bancos)
            };

            return View("ContaCorrente", contaCorrente);
        }

        public IActionResult Banco()
        {
            return View("Banco");
        }

        public IActionResult HistoricoListar()
        {
            var listaHistorico = _context.HistoricosOfx.ToList();
            var historicoViewModel = new List<HistoricoOfxViewModel>();
            foreach (var historico in listaHistorico)
            {
                historicoViewModel.Add(new HistoricoOfxViewModel() 
                { 
                    Id = historico.Id,
                    Descricao = historico.Descricao,
                    CodigoContaDebitoSelecionada = historico.ContaDebitoId,
                    CodigoContaCreditoSelecionada = historico.ContaCreditoId
                });
            }
            return View("HistoricoListar", historicoViewModel);
        }

        public IActionResult HistoricoCriar()
        {
            var contasContabeis = _context.ContasContabeis.ToList();

            var historico = new HistoricoOfxViewModel()
            {
                ContaCreditoId = ConstruirContasContabeisSelectList(contasContabeis),
                ContaDebitoId = ConstruirContasContabeisSelectList(contasContabeis)
            };

            return View("HistoricoCriar", historico);
        }
        public IActionResult HistoricoEditar(int id)
        {
            var historico = _context.HistoricosOfx.FirstOrDefault(x => x.Id == id);
            var contasContabeis = _context.ContasContabeis.ToList();

            return View("HistoricoEditar",new HistoricoOfxViewModel()
            {
                Id = historico.Id,
                Descricao = historico.Descricao,
                CodigoContaCreditoSelecionada = historico.ContaCreditoId,
                CodigoContaDebitoSelecionada = historico.ContaDebitoId,
                ContaCreditoId = ConstruirContasContabeisSelectList(contasContabeis),
                ContaDebitoId = ConstruirContasContabeisSelectList(contasContabeis)
            });
        }

        [HttpPost]
        public IActionResult HistoricoEditar(HistoricoOfxViewModel historico)
        {
            historico.ReturnUrl ??= Url.Content("~/");
            var updateHistorico = _context.HistoricosOfx.Find(Convert.ToInt32(historico.Id));

            updateHistorico.Descricao = historico.Descricao;
            updateHistorico.ContaDebitoId = historico.CodigoContaDebitoSelecionada;
            updateHistorico.ContaCreditoId = historico.CodigoContaCreditoSelecionada;

            _context.HistoricosOfx.Update(updateHistorico);
            _context.SaveChanges();

            return HistoricoListar();
        }

        [HttpPost]
        public IActionResult GravarOfx(ExtratoBancarioViewModel ofxs)
        {
            foreach (var ofx in ofxs.ContasCorrentes.LancamentosOfxs)
            {
                var descric = _context.HistoricosOfx.Any(h => h.Descricao == ofx.Description);
                var cc = _context.ConstasCorrentes.Any(c => c.NumeroConta == ofxs.ContasCorrentes.NumeroConta);
                var banco = _context.BancoOfxs.Any(b => b.Codigo == ofxs.Banco.Codigo);

                if (banco == false)
                {
                    //banco não cadastrado
                }

                if (cc == false)
                {
                    //Todo: adicionar Conta corrente
                    _context.ConstasCorrentes.Add(new ContaCorrente()
                    {
                        NumeroConta = ofxs.ContasCorrentes.NumeroConta,
                        EmpresaId = _context.Empresas.FirstOrDefault(e=> e.Codigo == ofxs.EmpresaSelecionada).Codigo,
                        BancoOfxId = _context.BancoOfxs.FirstOrDefault(b => b.Codigo == ofxs.Banco.Codigo).Id,
                    });
                    _context.SaveChanges();
                }
                if (descric == false)
                {
                    //Todo: adicionar histórico
                    _context.HistoricosOfx.Add(new HistoricoOfx()
                    {
                        Descricao = ofx.Description,
                        ContaCreditoId = ofx.ContaCreditoSelecionada,
                        ContaDebitoId = ofx.ContaDebitoSelecionada
                    });
                    _context.SaveChanges();
                }

                _context.Ofxs.Add(new LancamentoOfx()
                {
                    Documento = ofx.Id,
                    TipoLancamento = ofx.Type,
                    Descricao = ofx.Description,
                    ValorOfx = ofx.TransationValue,
                    Data = ofx.Date,
                    HistoricoOfxId = _context.HistoricosOfx.FirstOrDefault(h => h.Descricao == ofx.Description).Id,
                    ContaCorrenteId = _context.ConstasCorrentes.FirstOrDefault(c => c.NumeroConta == ofxs.ContasCorrentes.NumeroConta).Id
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

        private static SelectList ContruirBancos(IEnumerable<BancoOfx> bancos)
            => new(bancos.Select(c => new { c.Codigo, Nome = $"{c.Codigo} - {c.Nome}" }), "Codigo", "Nome");

    }
}
