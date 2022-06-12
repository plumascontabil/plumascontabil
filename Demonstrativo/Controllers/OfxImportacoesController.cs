using Demonstrativo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OFXParser;
using OFXParser.Entities;
using OFXSharp;
using Microsoft.EntityFrameworkCore;
using DomainService;

namespace Demonstrativo.Controllers
{
    public class OfxImportacoesController : BaseController
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _appEnvironment;
        // private readonly OfxImportacoesDomainService _ofxImportacoesDomainService;


        public OfxImportacoesController(Context context,
            IWebHostEnvironment env
            //OfxImportacoesDomainService ofxImportacoesDomainService
            ) : base(context)
        {
            _context = context;
            _appEnvironment = env;
            //_ofxImportacoesDomainService = ofxImportacoesDomainService;
        }

        public IActionResult Index()
        {
            IniT();

            return View();
        }

        private void IniT()
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();

            if (ViewBag.EmpresaSeleciodaId != null)
            {
                var idEmp = Convert.ToInt32($"{ViewBag.EmpresaSeleciodaId}");
                var contaCorrente = _context.ContasCorrentes.Where(f => f.EmpresaId == idEmp).ToList();
                ViewBag.BancosId = contaCorrente;
                ViewBag.Lotes = _context.OfxLoteLancamento.Where(f => f.EmpresaId == idEmp).ToList().OrderByDescending(f => f.Data).ToList();
            }
            else
            {
                ViewBag.BancosId = new List<OfxContaCorrente>();
                ViewBag.Lotes = new List<OfxLoteLancamento>();
            }



        }

        [HttpPost]
        public async Task<IActionResult> OfxLoteDelete(int LoteLancamentoId)
        {
            IniT();
            var lote = _context.OfxLoteLancamento.Where(f => f.Id == LoteLancamentoId).FirstOrDefault();
            var lancamentos = _context.OfxLancamentos.Where(f => f.LoteLancamentoId == lote.Id).ToList();
            var saldo = _context.SaldoMensal.Where(f => f.Competencia == lote.CompetenciaId && f.ContaCorrenteId == lancamentos.FirstOrDefault().ContaCorrenteId).FirstOrDefault();
            saldo.Saldo -= lote.Valor;

            lancamentos.ForEach(el =>
            {
                _context.OfxLancamentos.Remove(el);
            });
            _context.SaveChanges();
            _context.Remove(lote);
            _context.SaveChanges();
            ViewBag.Sucesso = "Lote Deletado com Sucesso";
            IniT();
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> OfxImportar(IFormFile ofxArquivo = null, int? BancosId = null, string DescricaoLote = null)
        {
            IniT();

            if (ViewBag.EmpresaSeleciodaId == null)
            {

                ViewBag.Message = "Porfavor, Selecione uma empresa, localizada no topo e Filtre!";
                return View("Index");

            }

            if (ViewBag.CompetenciasSelecionadaId == null)
            {
                // IniT();
                ViewBag.Message = "Porfavor, Selecione uma competência, localizada no topo e Filtre!";
                return View("Index");

            }
            //Listas
            var empresas = _context.Empresas.ToList();
            var contasContabeis = _context.ContasContabeis.ToList();
            var lancamentosPadroes = _context.LancamentosPadroes.ToList();
            var autoDescricoes = _context.AutoDescricoes;

            if (ofxArquivo == null)
            {
                // IniT();
                ViewBag.SemArquivo = "É necessário escolher um arquivo para envio do OFX!";
                return View("Index");
            }

            if (!ofxArquivo.FileName.Contains("ofx"))
            {
                // IniT();
                ViewBag.SemArquivo = $"Esse arquivo não possui o formato correto! Importe um arquivo OFX.";
                return View("Index");
            }

            if (!BancosId.HasValue)
            {
                // IniT();
                ViewBag.SemBanco = "É necessário escolher um banco para envio do OFX!";
                return View("Index");
            }

            if (string.IsNullOrEmpty(DescricaoLote))
            {
                ViewBag.Message = "Descrição do Lote é obrigatória!";
                return View("Index");
            }

            var bancoEscolhido = _context.OfxBancos.FirstOrDefault(f => f.Id == BancosId.Value);

            //Views Models
            var lancamentoOfxViewModel = new List<OfxLancamentoViewModel>();
            var contaCorrenteViewModel = new OfxContaCorrenteViewModel();
            var extratoBancarioViewModel = new ExtratoBancarioViewModel();
            var saldoMensalViewModel = new SaldoMensalViewModel();

            decimal saldo = 0;
            //If Leitura arquivo não nulo
            if (ofxArquivo != null)
            {

                //Caminho para salvar arquivo no servidor
                string caminhoDestinoArquivo = $"{_appEnvironment.WebRootPath}\\Temp\\{ofxArquivo.FileName}";

                using (var stream = new FileStream(caminhoDestinoArquivo, FileMode.Create))
                {
                    await ofxArquivo.CopyToAsync(stream);
                }
                //Extraindo conteudo do arquivo em um objeto do tipo Extract
                Extract extratoBancario = Parser.GenerateExtract(caminhoDestinoArquivo);
                if (extratoBancario != null)
                {
                    var documento = new OFXDocumentParser();
                    var dadoDocumento = documento.Import(new FileStream(caminhoDestinoArquivo, FileMode.Open));
                    saldo = dadoDocumento.Balance.LedgerBalance;
                }

                var dadosContaCorrente = _context.ContasCorrentes.FirstOrDefault(c => c.NumeroConta == extratoBancario.BankAccount.AccountCode);
                if (dadosContaCorrente != null)
                {
                    saldoMensalViewModel = new SaldoMensalViewModel()
                    {
                        SaldoMensal = saldo,
                        Competencia = extratoBancario.InitialDate,
                        ContaCorrenteId = dadosContaCorrente.Id,
                    };
                }
                else
                {
                    // IniT();
                    ViewBag.ContaCorrenteNaoEncontrada = true;
                    return View("Index");
                }


                //varrendo arquivo e adicionado as ViewsModel
                foreach (var dados in extratoBancario.Transactions)
                //foreach (var dados in extratoBancario.Transactions)
                {
                    var banco = _context.OfxBancos
                        .FirstOrDefault(b => b.Codigo == Convert.ToInt32(extratoBancario.BankAccount.Bank.Code));

                    if (banco == null)
                    {
                        ViewBag.SemBanco = "Banco Inexistente";
                        break;
                    }

                    if (banco.Id != bancoEscolhido.Id)
                    {
                        ViewBag.SemBanco = "O Banco do Arquivo não confere com o banco escolhido";
                        break;
                    }
                    var bancoViewModel = new OfxBancoViewModel()
                    {
                        Id = banco.Id,
                        Codigo = banco.Codigo,
                        Nome = banco.Nome
                    };

                    if (_context.OfxLancamentos.ToList().Any(c => c.Documento == dados.Id) == false)
                    {
                        if (autoDescricoes.FirstOrDefault(a => a.Descricao == dados.Description) == null)
                        {
                            lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
                            {
                                Id = dados.Id,
                                TransationValue = Convert.ToDecimal(dados.TransactionValue),
                                Description = dados.Description,
                                Date = dados.Date,
                                CheckSum = dados.Checksum,
                                Type = dados.Type,
                                SaldoMensal = saldoMensalViewModel,
                                LancamentosPadroes = ConstruirLancamentosPadroesSelectList(lancamentosPadroes)
                            });
                        }
                        else
                        {
                            lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
                            {
                                Id = dados.Id,
                                TransationValue = Convert.ToDecimal(dados.TransactionValue),
                                Description = dados.Description,
                                Date = dados.Date,
                                CheckSum = dados.Checksum,
                                Type = dados.Type,
                                SaldoMensal = saldoMensalViewModel,
                                LancamentosPadroes = ConstruirLancamentosPadroesSelectList(lancamentosPadroes),
                                LancamentoPadraoSelecionado = autoDescricoes.FirstOrDefault(a => a.Descricao == dados.Description).LancamentoPadraoId
                            });
                        }

                        contaCorrenteViewModel = new OfxContaCorrenteViewModel()
                        {
                            OfxLancamentos = lancamentoOfxViewModel,
                            NumeroConta = extratoBancario.BankAccount.AccountCode,
                        };

                        extratoBancarioViewModel = new ExtratoBancarioViewModel()
                        {
                            Empresas = ConstruirEmpresas(empresas),
                            ContasCorrentes = contaCorrenteViewModel,
                            Banco = bancoViewModel,
                            DescricaoLote = DescricaoLote

                        };
                    }
                    else
                    {
                        break;
                    }
                }
                if (ViewBag.SemBanco != null)
                {
                    // IniT();
                    return View("Index");

                }
                extratoBancarioViewModel.EmpresaSelecionada = ViewBag.EmpresaSeleciodaId;

                //Deletando arquivo do servidor
                System.IO.File.Delete(caminhoDestinoArquivo);
                System.IO.File.Delete($"{caminhoDestinoArquivo}.xml");
            }
            if (extratoBancarioViewModel.Banco == null)
            {
                // IniT();
                ViewBag.Message = "Este arquivo já foi importado!";
                return View("Index");
            }
            else return View("Contas", extratoBancarioViewModel);
        }

        [HttpPost]
        public IActionResult OfxReimportar(ExtratoBancarioViewModel extratoViewModel = null)
        {

            //var extratoBancarioViewModel = _ofxImportacoesDomainService.OfxReimportar(extratoViewModel);

            //Listas
            var empresas = _context.Empresas.ToList();
            var contasContabeis = _context.ContasContabeis.ToList();
            var lancamentosPadroes = _context.LancamentosPadroes.ToList();
            var autoDescricoes = _context.AutoDescricoes;

            //Views Models
            var lancamentoOfxViewModel = new List<OfxLancamentoViewModel>();
            var contaCorrenteViewModel = new OfxContaCorrenteViewModel();
            var extratoBancarioViewModel = new ExtratoBancarioViewModel();

            foreach (var dados in extratoViewModel.ContasCorrentes.OfxLancamentos)
            {
                var lancamentoPadrao = lancamentosPadroes
                    .FirstOrDefault(h => h.Descricao == extratoViewModel.LancamentoManual.Descricao);

                if (lancamentoPadrao == null)
                {
                    lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
                    {
                        Id = dados.Id,
                        TransationValue = dados.TransationValue,
                        Description = dados.Description,
                        Date = dados.Date,
                        CheckSum = dados.CheckSum,
                        Type = dados.Type,
                        LancamentosPadroes = ConstruirLancamentosPadroesSelectList(lancamentosPadroes)
                    });
                }
                else
                {
                    lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
                    {
                        Id = dados.Id,
                        TransationValue = dados.TransationValue,
                        Description = dados.Description,
                        Date = dados.Date,
                        CheckSum = dados.CheckSum,
                        Type = dados.Type,
                        LancamentosPadroes = ConstruirLancamentosPadroesSelectList(lancamentosPadroes),
                        LancamentoPadraoSelecionado =
                            Convert.ToInt32(_context.LancamentosPadroes
                                            .FirstOrDefault(l => l.Codigo == lancamentoPadrao.Codigo)
                                                .Codigo)
                    });
                }

                var banco = _context.OfxBancos.FirstOrDefault(b => b.Codigo == extratoViewModel.Banco.Codigo);

                var bancoViewModel = new OfxBancoViewModel()
                {
                    Id = banco.Id,
                    Codigo = banco.Codigo,
                    Nome = banco.Nome
                };

                contaCorrenteViewModel = new OfxContaCorrenteViewModel()
                {
                    OfxLancamentos = lancamentoOfxViewModel,
                    NumeroConta = extratoViewModel.ContasCorrentes.NumeroConta
                };
                extratoBancarioViewModel = new ExtratoBancarioViewModel()
                {
                    Empresas = ConstruirEmpresas(empresas),
                    ContasCorrentes = contaCorrenteViewModel,
                    Banco = bancoViewModel
                };
            }

            if (extratoViewModel.LancamentoManual != null)
            {
                var ids = extratoViewModel.ContasCorrentes.OfxLancamentos.Max(x => x.Id) + 1;
                lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
                {
                    TransationValue = extratoViewModel.LancamentoManual.Valor,
                    Description = extratoViewModel.LancamentoManual.Descricao,
                    Date = extratoViewModel.LancamentoManual.Data,
                    CheckSum = 1,
                    Type = extratoViewModel.LancamentoManual.TipoSelecionado,
                    LancamentosPadroes = ConstruirLancamentosPadroesSelectList(lancamentosPadroes),
                    SaldoMensal = new SaldoMensalViewModel(),

                    Id = ids

                });
            }
            extratoBancarioViewModel.DescricaoLote = extratoViewModel.DescricaoLote;
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            //
            return View("Contas", extratoBancarioViewModel);
        }

        [HttpPost]
        public IActionResult OfxSalvar(ExtratoBancarioViewModel dados)
        {
            //var extratoBancarioViewModel = _ofxImportacoesDomainService.OfxSalvar(dados);


            //if (dados.ContasCorrentes.OfxLancamentos.Any(x => x.LancamentoPadraoSelecionado == 0))
            //{
            //    ViewBag.LancamentoPadraoSelecionadoNotSelect = "Existe lançamentos sem ser atribuidos a contas contábeis";
            //    return View("Contas", dados);
            //}
            IniT();
            dados.EmpresaSelecionada = ViewBag.EmpresaSeleciodaId;
            if (dados.EmpresaSelecionada == 0)
            {
                ViewBag.LancamentoPadraoSelecionadoNotSelect = "Selecione a empresa";
                return View("Contas", dados);
            }

            decimal saldoMensalTotal = 0;

            // Fazer Toda a parte do Lote

            var lote = new OfxLoteLancamento()
            {
                Data = DateTime.Now,
                Descricao = dados.DescricaoLote,
                Valor = 0,
                EmpresaId = ViewBag.EmpresaSeleciodaId,
                CompetenciaId = Convert.ToDateTime(ViewBag.CompetenciasSelecionadaId)
            };
            dados.ContasCorrentes.OfxLancamentos.ForEach(el =>
            {
                lote.Valor += el.TransationValue;
            });
            _context.OfxLoteLancamento.Add(lote);
            _context.SaveChanges();

            foreach (var dado in dados.ContasCorrentes.OfxLancamentos)
            {
                var cc = _context.ContasCorrentes.Any(c => c.NumeroConta == dados.ContasCorrentes.NumeroConta);
                var banco = _context.OfxBancos.Any(b => b.Codigo == dados.Banco.Codigo);
                var lancamentoPadrao = _context.LancamentosPadroes.FirstOrDefault(c => c.Codigo == dado.LancamentoPadraoSelecionado);
                if (banco == false)
                {
                    //banco não cadastrado
                }

                if (cc == false)
                {
                    //Todo: adicionar Conta corrente
                    _context.ContasCorrentes.Add(new OfxContaCorrente()
                    {
                        NumeroConta = dados.ContasCorrentes.NumeroConta,
                        EmpresaId = _context.Empresas.FirstOrDefault(e => e.Codigo == dados.EmpresaSelecionada).Codigo,
                        BancoOfxId = _context.OfxBancos.FirstOrDefault(b => b.Codigo == dados.Banco.Codigo).Id,
                    });
                    _context.SaveChanges();
                }

                var contaCorrente = _context.ContasCorrentes.FirstOrDefault(c => c.NumeroConta == dados.ContasCorrentes.NumeroConta);

                var saldoMensalId = _context.SaldoMensal.FirstOrDefault(s => s.Competencia == Convert.ToDateTime(dado.Date.ToString("yyyy/MM"))
                                                                        && s.ContaCorrenteId == contaCorrente.Id);
                if (saldoMensalId == null)
                {
                    _context.SaldoMensal.Add(new SaldoMensal()
                    {
                        Competencia = Convert.ToDateTime(ViewBag.CompetenciasSelecionadaId),// dado.SaldoMensal.Competencia,
                        Saldo = dado.SaldoMensal.SaldoMensal == 0 ? lote.Valor : dado.SaldoMensal.SaldoMensal,
                        ContaCorrenteId = dado.SaldoMensal.ContaCorrenteId
                    });
                }
                else
                {
                    saldoMensalId.Saldo += dado.SaldoMensal.SaldoMensal == 0 ? lote.Valor : dado.SaldoMensal.SaldoMensal;
                }

                _context.OfxLancamentos.Add(new OfxLancamento()
                {
                    Documento = dado.Id,
                    TipoLancamento = dado.Type,
                    Descricao = dado.Description,
                    ValorOfx = dado.TransationValue,
                    Data = dado.Date,
                    LoteLancamentoId = lote.Id,
                    ContaCorrenteId = _context.ContasCorrentes
                        .FirstOrDefault(c => c.NumeroConta == dados.ContasCorrentes.NumeroConta).Id,
                    LancamentoPadraoId = _context.LancamentosPadroes.FirstOrDefault(l => l.Codigo == dado.LancamentoPadraoSelecionado).Id
                });
                _context.SaveChanges();
                var descricao = _context.AutoDescricoes
                    .FirstOrDefault(c => c.Descricao == dado.Description);
                if (descricao == null)
                {
                    _context.AutoDescricoes.Add(new AutoDescricao()
                    {
                        Descricao = dado.Description,
                        LancamentoPadraoId = Convert.ToInt32(lancamentoPadrao.Id)
                    });
                    _context.SaveChanges();
                }
                else if (descricao.LancamentoPadraoId == lancamentoPadrao.Id
                    && descricao.Descricao != dado.Description)
                {
                    _context.AutoDescricoes.Add(new AutoDescricao()
                    {
                        Descricao = dado.Description,
                        LancamentoPadraoId = lancamentoPadrao.Id
                    });
                    _context.SaveChanges();
                }
                else
                {
                    continue;
                }
            }

            _context.SaveChanges();
            ViewBag.Importado = "Arquivo Importado!";
            IniT();
            return View("Index");
        }
        private static SelectList ConstruirLancamentosPadroesSelectList(IEnumerable<LancamentoPadrao> lancamentoPadroes)
            => new(lancamentoPadroes.Select(c => new { c.Codigo, Descricao = $"{c.Codigo} - {c.Descricao}" }), "Codigo", "Descricao");
        private static SelectList ConstruirEmpresas(IEnumerable<Empresa> empresas)
            => new(empresas.Select(e => new { e.Codigo, Razao = $"{e.Codigo} - {e.RazaoSocial}" }), "Codigo", "Razao");
    }
}
//else
//{
//    foreach (var dados in extratoView.ContasCorrentes.LancamentosOfxs)
//    {
//        var historico = historicos.FirstOrDefault(h => h.Descricao == extratoView.LancamentoManual.Descricao);

//        if (historico == null)
//        {
//            lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
//            {
//                Id = dados.Id,
//                TransationValue = dados.TransationValue,
//                Description = dados.Description,
//                Date = dados.Date,
//                CheckSum = dados.CheckSum,
//                Type = dados.Type,
//                ContasCredito = ConstruirContasContabeisSelectList(contasContabeis),
//                ContasDebito = ConstruirContasContabeisSelectList(contasContabeis),
//            });
//        }
//        else
//        {
//            lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
//            {
//                TransationValue = extratoView.LancamentoManual.Valor,
//                Description = extratoView.LancamentoManual.Descricao,
//                Date = extratoView.LancamentoManual.Data,
//                CheckSum = 1,
//                Type = extratoView.LancamentoManual.TipoSelecionado,
//                ContaCreditoSelecionada = historico.ContaCreditoId,
//                ContaDebitoSelecionada = historico.ContaDebitoId,
//                HistoricoId = historico.Id,
//                ContasCredito = ConstruirContasContabeisSelectList(contasContabeis.Where(x => x.Codigo == historico.ContaCreditoId)),
//                ContasDebito = ConstruirContasContabeisSelectList(contasContabeis.Where(x => x.Codigo == historico.ContaDebitoId)),
//            });
//        }

//        var banco = _context.OfxBancos.FirstOrDefault(b => b.Codigo == extratoView.Banco.Codigo);

//        var bancoViewModel = new OfxBancoViewModel()
//        {
//            Id = banco.Id,
//            Codigo = banco.Codigo,
//            Nome = banco.Nome
//        };

//        contaCorrenteViewModel = new OfxContaCorrenteViewModel()
//        {
//            LancamentosOfxs = lancamentoOfxViewModel,
//            NumeroConta = extratoView.ContasCorrentes.NumeroConta
//        };
//        extratoBancarioViewModel = new ExtratoBancarioViewModel()
//        {
//            Empresas = ConstruirEmpresas(empresas),
//            ContasCorrentes = contaCorrenteViewModel,
//            Banco = bancoViewModel
//        };
//    }

//    if (extratoView.LancamentoManual != null)
//    {
//        lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
//        {
//            TransationValue = extratoView.LancamentoManual.Valor,
//            Description = extratoView.LancamentoManual.Descricao,
//            Date = extratoView.LancamentoManual.Data,
//            CheckSum = 1,
//            Type = extratoView.LancamentoManual.TipoSelecionado,
//            ContasCredito = ConstruirContasContabeisSelectList(contasContabeis),
//            ContasDebito = ConstruirContasContabeisSelectList(contasContabeis),
//        });
//    }
//}