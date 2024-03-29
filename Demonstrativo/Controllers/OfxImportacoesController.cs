﻿using Demonstrativo.Models;
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

namespace Demonstrativo.Controllers
{
    public class OfxImportacoesController : Controller
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public OfxImportacoesController(Context context, IWebHostEnvironment env)
        {
            _context = context;
            _appEnvironment = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OfxImportar(IFormFile ofxArquivo = null)
        {
            //Listas
            var empresas = _context.Empresas.ToList();
            var contasContabeis = _context.ContasContabeis.ToList();
            var lancamentosPadroes = _context.LancamentosPadroes.ToList();
            var autoDescricoes = _context.AutoDescricoes;

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
                if(extratoBancario != null)
                {
                    var documento = new OFXDocumentParser();
                    var dadoDocumento = documento.Import(new FileStream(caminhoDestinoArquivo, FileMode.Open));
                    saldo = dadoDocumento.Balance.LedgerBalance;
                }

                saldoMensalViewModel = new SaldoMensalViewModel()
                {
                    SaldoMensal = saldo,
                    Competencia = extratoBancario.InitialDate,
                    ContaCorrenteId = _context.ContasCorrentes
                        .FirstOrDefault(c => c.NumeroConta == extratoBancario.BankAccount.AccountCode).Id,
                };

                //varrendo arquivo e adicionado as ViewsModel
                foreach (var dados in extratoBancario.Transactions)
                //foreach (var dados in extratoBancario.Transactions)
                {
                    var banco = _context.OfxBancos
                        .FirstOrDefault(b => b.Codigo == Convert.ToInt32(extratoBancario.BankAccount.Bank.Code));

                    if (banco == null) 
                    { 
                        ViewBag.AdicionarBanco = "Banco Inexistente"; 
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
                                TransationValue = dados.TransactionValue,
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
                                TransationValue = dados.TransactionValue,
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
                            Banco = bancoViewModel
                        };
                    }
                    else
                    {
                        break;
                    }
                }
                //Deletando arquivo do servidor
                System.IO.File.Delete(caminhoDestinoArquivo);
                System.IO.File.Delete($"{caminhoDestinoArquivo}.xml");
            }
            if (extratoBancarioViewModel.Banco == null) return View("Index", ViewBag.Message = "Este arquivo já foi importado!");
            else return View("Contas", extratoBancarioViewModel);
        }

        [HttpPost]
        public IActionResult OfxReimportar(ExtratoBancarioViewModel extratoViewModel = null) 
        {
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
                lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
                {
                    TransationValue = extratoViewModel.LancamentoManual.Valor,
                    Description = extratoViewModel.LancamentoManual.Descricao,
                    Date = extratoViewModel.LancamentoManual.Data,
                    CheckSum = 1,
                    Type = extratoViewModel.LancamentoManual.TipoSelecionado,
                    LancamentosPadroes = ConstruirLancamentosPadroesSelectList(lancamentosPadroes)
                });
            }
            return View("Contas", extratoBancarioViewModel);
        }

        [HttpPost]
        public IActionResult OfxSalvar(ExtratoBancarioViewModel dados)
        {
            decimal saldoMensalTotal = 0;
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
                        Competencia = dado.SaldoMensal.Competencia,
                        Saldo=dado.SaldoMensal.SaldoMensal,
                        ContaCorrenteId = dado.SaldoMensal.ContaCorrenteId
                    });
                }

                _context.OfxLancamentos.Add(new OfxLancamento()
                {
                    Documento = dado.Id,
                    TipoLancamento = dado.Type,
                    Descricao = dado.Description,
                    ValorOfx = dado.TransationValue,
                    Data = dado.Date,
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

            return View("Index", ViewBag.Importado = "Arquivo Importado!");
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