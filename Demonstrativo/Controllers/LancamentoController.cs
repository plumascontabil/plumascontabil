using Demonstrativo.Models;
using DomainService;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using Microsoft.Extensions.Logging;

namespace Demonstrativo.Controllers
{
    [Authorize]
    public class LancamentoController : BaseController
    {
        readonly Context _context;
        private readonly ILogger<Lancamento> _logger;

        //private readonly LancamentoDomainService _lancamentoDomainService;

        public LancamentoController(Context context,
            ILogger<Lancamento> logger,
            RoleManager<IdentityRole> roleManager) : base(context, roleManager)
        {
            _context = context;
            _logger = logger;
            //_lancamentoDomainService = LancamentoDomainService;
        }
        public IActionResult Dre()
        {
            AdicionarCompetenciaMesAtual();

            CarregarEmpresasCompetencias();
            if (ViewBag.EmpresaSeleciodaId == null || ViewBag.CompetenciasSelecionadaId == null)
            {
                ViewBag.Message = "Porfavor, selecione uma empresa e uma competencia e filtre!";
                return View("Index", CarregarCategorias());
            }
            var date = Convert.ToDateTime(ViewBag.CompetenciasSelecionadaId);

            return View("Dre", CarregarCategorias((int?)ViewBag.EmpresaSeleciodaId, (DateTime?)date));
        }
        public IActionResult Index()
        {
            ViewBag.Qualquer = 10;
            AdicionarCompetenciaMesAtual();

            CarregarEmpresasCompetencias();
            var date = (DateTime?)null;

            if (ViewBag.CompetenciasSelecionadaId != null)
            {
                date = Convert.ToDateTime(ViewBag.CompetenciasSelecionadaId);
            }
            ViewBag.Sucesso = TempData["Sucesso"];

            return View(CarregarCategorias((int?)ViewBag.EmpresaSeleciodaId, (DateTime?)date));
        }

        //private void AdicionarCompetenciaMesAtual()
        //{
        //    DateTime competenciaAtual = new(DateTime.Now.Year, DateTime.Now.Month, 01);

        //    if (_context.Competencias.Any(c => c.Data == competenciaAtual))
        //    {
        //        return;
        //    }

        //    var competencia = new Competencia()
        //    {
        //        Data = competenciaAtual
        //    };

        //    _context.Competencias.Add(competencia);
        //    _context.SaveChanges();

        //    //_lancamentoDomainService.AdicionarCompetenciaMesAtual();
        //}

        //private void CarregarEmpresasCompetencias(int? empresaId = null, DateTime? competenciasId = null)
        //{
        //    List<Empresa> empresas = _context.Empresas.ToList();
        //    List<Competencia> competencias = _context.Competencias.ToList();

        //    //_lancamentoDomainService.CarregarEmpresas();
        //    //_lancamentoDomainService.CarregarCompetencias();


        //    ViewBag.CompetenciasId = new SelectList(
        //            competencias.Select(
        //            c => new { Value = c.Data.ToShortDateString(), Text = c.Data.ToString("MM/yyyy") })
        //            , "Value", "Text",
        //            competenciasId.HasValue ? competenciasId.Value.ToShortDateString() : competenciasId);

        //    ViewBag.EmpresasId = new SelectList(empresas.Select(F => new { Value = F.Codigo, Text = $"{F.Codigo} - {F.RazaoSocial}" }).ToList(), "Value", "Text", empresaId);
        //}

        private TrimestreViewModel CarregarCategorias(int? empresasId = null, DateTime? competenciasId = null)
        {
            // var trimestreViewModel = await _lancamentoDomainService.CarregarCategorias(empresasId, competenciasId);

            var trimestreViewModel = new TrimestreViewModel();
            trimestreViewModel.CompetenciaSelecionadaId = ReturnCompetenciaMesAtual();
            var contas = _context.LancamentosPadroes.ToList();
            var categorias = _context.Categorias.ToList();
            var contasCorrentes = _context.ContasCorrentes.Where(c => c.EmpresaId == empresasId).ToList();
            var autoDescricao = _context.AutoDescricoes.ToList();

            trimestreViewModel.EmpresaSelecionada = ViewBag.EmpresaSeleciodaId ?? 0;
            trimestreViewModel.Empresas = ViewBag.EmpresasId;
            if (ViewBag.CompetenciaSelecionadaId != null)
                trimestreViewModel.CompetenciaSelecionadaId = ViewBag.CompetenciaSelecionadaId ?? null;
            trimestreViewModel.Competencias = ViewBag.CompetenciasId;


            var lancamentos = new List<Lancamento>();

            if (empresasId.HasValue && competenciasId.HasValue)
            {
                lancamentos = _context.Lancamentos.Where(x => x.EmpresaId == empresasId && x.DataCompetencia == competenciasId)
                    .ToList();
            }
            var contasCorrentesLancamentos = new List<OfxLancamento>();

            if (empresasId.HasValue)
            {
                var ids = contasCorrentes.Select(x => x.Id).ToList();

                contasCorrentesLancamentos = _context.OfxLancamentos.Where(o => ids.Any(x => x == o.ContaCorrenteId) == true).ToList();
            }


            categorias.ForEach(categoria =>
            {
                var contasViewModel = new List<ContaViewModel>();
                contas.Where(c => c.CategoriaId == categoria.Id).ToList().ForEach(conta =>
                {
                    var lancamentosViewModel = new List<LancamentoViewModel>();
                    decimal valor = 0;
                    contasCorrentes.ForEach(contaCorrente =>
                    {
                        var ofxLancamentos = contasCorrentesLancamentos.Where(f => f.ContaCorrenteId == contaCorrente.Id).ToList();

                        if (conta.Codigo == 200)
                        {
                            var saldoBanco = _context.SaldoMensal.FirstOrDefault(c => c.Competencia == competenciasId && c.ContaCorrenteId == contaCorrente.Id);



                            lancamentosViewModel.Add(new LancamentoViewModel()
                            {
                                Valor = saldoBanco != null ? saldoBanco.Saldo : 0,
                                Descricao = _context.OfxBancos.FirstOrDefault(c => c.Id == contaCorrente.BancoOfxId).Nome
                            });
                        }

                        ofxLancamentos
                          .Where(l => l.Data.Year == competenciasId.Value.Year
                                  && l.Data.Month == competenciasId.Value.Month).ToList().ForEach(ofxLancamento =>
                                  {
                                      var contaCodigo = autoDescricao.FirstOrDefault(a => a.Descricao == ofxLancamento.Descricao).LancamentoPadraoId;
                                      if (ofxLancamento != null && contaCodigo == conta.Id && contaCodigo != 200)
                                      {
                                          valor += ofxLancamento.ValorOfx;
                                      }
                                  });



                    });


                    if (valor != 0)
                    {
                        lancamentosViewModel.Add(new LancamentoViewModel()
                        {
                            Valor = Convert.ToDecimal(valor)
                        });
                    }


                    if (!lancamentosViewModel.Any())
                    {
                        lancamentosViewModel.Add(new LancamentoViewModel());
                    }

                    contasViewModel.Add(new ContaViewModel()
                    {
                        Id = conta.Id,
                        Codigo = conta.Codigo,
                        Descricao = conta.Descricao,
                        TipoLancamento = conta.TipoLancamento,
                        Lancamentos = lancamentosViewModel
                    });

                });

                trimestreViewModel.Categorias.Add(new CategoriaViewModel()
                {
                    Descricao = categoria.Descricao,
                    Contas = contasViewModel
                });
            });


            var trimestre = CarregarTrimestre(competenciasId, empresasId);
            var estorqueVenda = CarregarVenda(competenciasId, empresasId);

            trimestreViewModel.LancamentosCompra = trimestre.LancamentosCompra;
            trimestreViewModel.LancamentosReceita = trimestre.LancamentosReceita;
            trimestreViewModel.LancamentosDespesa = trimestre.LancamentosDespesa;
            trimestreViewModel.Trimestre = trimestre.Trimestre;
            trimestreViewModel.ProvisoesDepreciacoes = trimestre.ProvisoesDepreciacoes;
            trimestreViewModel.EstoqueVendas = estorqueVenda.EstoqueVendas;

            return trimestreViewModel;
        }

        [HttpPost]
        public IActionResult Filtrar(int empresasId, DateTime competenciasId)
        {
            CarregarEmpresasCompetencias(empresasId, competenciasId);



            return View("Index", CarregarCategorias(empresasId, competenciasId));
        }

        [HttpPost]
        public IActionResult Salvar(TrimestreViewModel trimestreViewModel)
        {
            AdicionarCompetenciaMesAtual();

            CarregarEmpresasCompetencias();
            if (ViewBag.EmpresaSeleciodaId == null || ViewBag.CompetenciasSelecionadaId == null)
            {
                ViewBag.Message = "Porfavor, selecione uma empresa e uma competencia e filtre!";
                return View("Index", CarregarCategorias());
            }
            DateTime competencia = Convert.ToDateTime(ViewBag.CompetenciasSelecionadaId);
            var empresaId = Convert.ToInt32($"{ViewBag.EmpresaSeleciodaId}");
            //var primeiroLancamento = await_lancamentoDomainService.Salvar(competencia, trimestreViewModel);

            var lancamentoCompetencia = _context.Lancamentos.Any(l => l.DataCompetencia == competencia && l.EmpresaId == empresaId);
            var estoqueVendas = trimestreViewModel.EstoqueVendas;

            if (lancamentoCompetencia == false)
            {
                var insertEstoqueVendas = new Venda()
                {
                    DataCompetencia = ViewBag.CompetenciasSelecionadaId,
                    EmpresaId = ViewBag.EmpresaSeleciodaId,
                    Observacao = estoqueVendas.Observacao
                };

                _context.Vendas.Add(insertEstoqueVendas);
                _context.SaveChanges();

                foreach (var itemVenda in estoqueVendas.ItensVendas)
                {
                    if (itemVenda.Id == 0 && itemVenda.Quantidade == 0 || itemVenda.Preco == 0)
                    {
                        continue;
                    }

                    if (itemVenda.Id == 0)
                    {
                        var insertItemVenda = new ItemVenda()
                        {
                            VendaId = insertEstoqueVendas.Id,
                            ProdutoId = itemVenda.ProdutoId,
                            Quantidade = itemVenda.Quantidade,
                            Preco = itemVenda.Preco
                        };

                        _context.ItensVendas.Add(insertItemVenda);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var updateItemVenda = _context.ItensVendas.Find(Convert.ToInt32(itemVenda.Id));

                        updateItemVenda.Quantidade = itemVenda.Quantidade;
                        updateItemVenda.Preco = itemVenda.Preco;

                        _context.ItensVendas.Update(updateItemVenda);
                        _context.SaveChanges();
                    }
                }
            }
            else
            {
                var updateEstoqueVendas = _context.Vendas.Find(Convert.ToInt32(estoqueVendas.Id));

                updateEstoqueVendas.DataCompetencia = competencia;
                updateEstoqueVendas.EmpresaId = empresaId;
                updateEstoqueVendas.Observacao = estoqueVendas.Observacao;

                foreach (var itemVenda in estoqueVendas.ItensVendas)
                {
                    if (itemVenda.Id == 0 && itemVenda.Quantidade == 0 || itemVenda.Preco == 0)
                    {
                        continue;
                    }

                    if (itemVenda.Id == 0)
                    {
                        var insertItemVenda = new ItemVenda()
                        {
                            VendaId = updateEstoqueVendas.Id,
                            ProdutoId = itemVenda.ProdutoId,
                            Quantidade = itemVenda.Quantidade,
                            Preco = itemVenda.Preco
                        };

                        _context.ItensVendas.Add(insertItemVenda);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var updateItemVenda = _context.ItensVendas.Find(Convert.ToInt32(itemVenda.Id));

                        updateItemVenda.Quantidade = itemVenda.Quantidade;
                        updateItemVenda.Preco = itemVenda.Preco;

                        _context.ItensVendas.Update(updateItemVenda);
                        _context.SaveChanges();
                    }
                }
            }

            var provisoesDepreciacoes = trimestreViewModel.ProvisoesDepreciacoes;
            var updateProvisoes = _context.ProvisoesDepreciacoes.Find(provisoesDepreciacoes.Id);
            if (lancamentoCompetencia == false || updateProvisoes == null)
            {
                var insertProvisoes = new ProvisoesDepreciacao()
                {
                    DataCompetencia = competencia,
                    EmpresaId = empresaId,
                    DecimoTerceiro = provisoesDepreciacoes.DecimoTerceiro,
                    Ferias = provisoesDepreciacoes.Ferias,
                    Depreciacao = provisoesDepreciacoes.Depreciacao,
                    SaldoPrejuizo = provisoesDepreciacoes.SaldoPrejuizo,
                    CalcularCompensacao = provisoesDepreciacoes.CalcularCompesacao,
                    Apurar = provisoesDepreciacoes.Apurar
                };

                _context.ProvisoesDepreciacoes.Add(insertProvisoes);
                _context.SaveChanges();
            }
            else
            {
                //var updateProvisoes = _context.ProvisoesDepreciacoes.Find(provisoesDepreciacoes.Id);

                updateProvisoes.DataCompetencia = competencia;//provisoesDepreciacoes.Data;
                updateProvisoes.EmpresaId = empresaId; //provisoesDepreciacoes.Empresa;
                updateProvisoes.DecimoTerceiro = provisoesDepreciacoes.DecimoTerceiro;
                updateProvisoes.Ferias = provisoesDepreciacoes.Ferias;
                updateProvisoes.Depreciacao = provisoesDepreciacoes.Depreciacao;
                updateProvisoes.SaldoPrejuizo = provisoesDepreciacoes.SaldoPrejuizo;
                updateProvisoes.CalcularCompensacao = provisoesDepreciacoes.CalcularCompesacao;
                updateProvisoes.Apurar = provisoesDepreciacoes.Apurar;

                _context.ProvisoesDepreciacoes.Update(updateProvisoes);
                _context.SaveChanges();
            }

            var lancamentosViewModel = trimestreViewModel.Categorias.SelectMany(x => x.Contas.SelectMany(x => x.Lancamentos)).ToList();

            var lancamentos = lancamentosViewModel.Select(x => new Lancamento()
            {
                Id = x.Id,
                ContaId = x.Conta,
                DataCompetencia = x.Data,
                Descricao = x.Descricao,
                EmpresaId = x.Empresa,
                Valor = x.Valor
            });

            foreach (var lancamento in lancamentos)
            {
                if (lancamento.Id == 0 && lancamento.Valor == 0)
                {
                    continue;
                }

                if (lancamento.Id != 0 && lancamento.Valor == 0)
                {
                    _context.Lancamentos.Remove(lancamento);
                    _context.SaveChanges();
                    continue;
                }

                if (lancamentoCompetencia)
                {
                    var insertLancamento = new Lancamento();
                    if (lancamento.Descricao == null || lancamento.ContaId == 156 || lancamento.ContaId == 98 || lancamento.ContaId == 157 || lancamento.ContaId == 140)
                    {
                        insertLancamento.ContaId = lancamento.ContaId;
                    }
                    insertLancamento.EmpresaId = lancamento.EmpresaId;
                    insertLancamento.DataCompetencia = lancamento.DataCompetencia;
                    insertLancamento.Descricao = lancamento.Descricao;
                    insertLancamento.Valor = lancamento.Valor;

                    _context.Lancamentos.Add(insertLancamento);
                    _context.SaveChanges();
                }
                else
                {
                    var updateLancamento = _context.Lancamentos.Find(Convert.ToInt32(lancamento.Id));

                    updateLancamento.Descricao = lancamento.Descricao;
                    updateLancamento.Valor = lancamento.Valor;

                    _context.Lancamentos.Update(updateLancamento);
                    _context.SaveChanges();
                }
            }

            var primeiroLancamento = lancamentos.FirstOrDefault();
            _logger.LogInformation(((int)EEventLog.Post), "Lançamento Id: {lancamento} created.", primeiroLancamento.Id);

            TempData["Sucesso"] = "Dados salvos com sucesso!";

            return RedirectToAction("Index");
        }

        public TrimestreViewModel CarregarTrimestre(DateTime? competenciasId = null, int? empresaId = null)
        {
            if (competenciasId == null)
            {
                return new TrimestreViewModel();
            }

            var mes = competenciasId.Value.Month;

            if (mes < 4)
            {
                int[] trimestre = { 1, 2, 3 };
                return SomarTrimestre(trimestre, empresaId, competenciasId);
            }
            else if (mes >= 4 && mes < 7)
            {
                int[] trimestre = { 4, 5, 6 };
                return SomarTrimestre(trimestre, empresaId, competenciasId);
            }
            else if (mes >= 7 && mes < 10)
            {
                int[] trimestre = { 7, 8, 9 };
                return SomarTrimestre(trimestre, empresaId, competenciasId);
            }
            else
            {
                int[] trimestre = { 10, 11, 12 };
                return SomarTrimestre(trimestre, empresaId, competenciasId);
            }
        }

        public TrimestreViewModel SomarTrimestre(int[] trimestre, int? empresaId, DateTime? competenciasId = null)
        {
            var trimestreViewModel = new TrimestreViewModel();

            var contasCorrentes = _context.ContasCorrentes.Where(c => c.EmpresaId == empresaId).ToList();
            var OfxLancamentos = _context.OfxLancamentos.Include(x => x.LancamentoPadrao).ToList();
            List<LancamentoPadrao> contas = _context.LancamentosPadroes.ToList();
            var provisaoDepreciacao = _context.ProvisoesDepreciacoes.Where(f => f.EmpresaId == empresaId && f.DataCompetencia == competenciasId.Value).FirstOrDefault();
            trimestreViewModel.Trimestre = trimestre;

            foreach (var competencia in trimestre)
            {
                foreach (var contaCorrente in contasCorrentes)
                {
                    foreach (var lancamento in OfxLancamentos.Where(l => l.Data.Month == competencia
                                                            && l.LancamentoPadrao?.TipoContaId == (int)ETipoConta.Compras
                                                            && l.ContaCorrenteId == contaCorrente.Id))
                    {
                        trimestreViewModel.LancamentosCompra.Add(new LancamentoViewModel()
                        {
                            Id = lancamento.Id,
                            Data = lancamento.Data,
                            Empresa = contaCorrente.EmpresaId,
                            Conta = lancamento.LancamentoPadraoId,
                            Descricao = lancamento.Descricao,
                            Valor = (decimal)lancamento.ValorOfx
                        });
                    }

                    foreach (var lancamento in OfxLancamentos.Where(l => l.Data.Month == competencia
                                                            && l.LancamentoPadrao?.TipoContaId == (int)ETipoConta.EstoqueInicial
                                                            && l.ContaCorrenteId == contaCorrente.Id))
                    {
                        trimestreViewModel.LancamentosCompra.Add(new LancamentoViewModel()
                        {
                            Id = lancamento.Id,
                            Data = lancamento.Data,
                            Empresa = contaCorrente.EmpresaId,
                            Conta = lancamento.LancamentoPadraoId,
                            Descricao = lancamento.Descricao,
                            Valor = (decimal)lancamento.ValorOfx
                        });
                    }

                    foreach (var lancamento in OfxLancamentos.Where(l => l.Data.Month == competencia
                                                            && l.LancamentoPadrao?.TipoContaId == (int)ETipoConta.EstoqueFinal
                                                            && l.ContaCorrenteId == contaCorrente.Id))
                    {
                        trimestreViewModel.LancamentosCompra.Add(new LancamentoViewModel()
                        {
                            Id = lancamento.Id,
                            Data = lancamento.Data,
                            Empresa = contaCorrente.EmpresaId,
                            Conta = lancamento.LancamentoPadraoId,
                            Descricao = lancamento.Descricao,
                            Valor = (decimal)lancamento.ValorOfx
                        });
                    }

                    foreach (var lancamento in OfxLancamentos.Where(l => l.Data.Month == competencia
                                                            && l.LancamentoPadrao?.TipoContaId == (int)ETipoConta.Receitas
                                                            && l.ContaCorrenteId == contaCorrente.Id))
                    {
                        trimestreViewModel.LancamentosReceita.Add(new LancamentoViewModel()
                        {
                            Id = lancamento.Id,
                            Data = lancamento.Data,
                            Empresa = contaCorrente.EmpresaId,
                            Conta = lancamento.LancamentoPadraoId,
                            Descricao = lancamento.Descricao,
                            Valor = (decimal)lancamento.ValorOfx
                        });
                    }

                    foreach (var conta in contas.Where(c => c.TipoContaId == (int)ETipoConta.Despesas))
                    {
                        if (conta.Lancamentos == null)
                        {
                            continue;
                        }

                        foreach (var lancamento in conta.Lancamentos.Where(l => l.EmpresaId == empresaId
                                                                        && l.DataCompetencia.Month == competencia))
                        {
                            trimestreViewModel.LancamentosDespesa.Add(new LancamentoViewModel()
                            {
                                Id = lancamento.Id,
                                Data = lancamento.DataCompetencia,
                                Empresa = lancamento.EmpresaId,
                                Conta = lancamento.ContaId,
                                Descricao = lancamento.Descricao,
                                Valor = lancamento.Valor
                            });
                        }
                    }
                }



                if (provisaoDepreciacao != null)
                {
                    trimestreViewModel.ProvisoesDepreciacoes = new ProvisoesDepreciacoesViewModel()
                    {
                        Id = provisaoDepreciacao.Id,
                        Data = provisaoDepreciacao.DataCompetencia,
                        Empresa = provisaoDepreciacao.EmpresaId,
                        FeriasVlr = provisaoDepreciacao.Ferias.ToString(),
                        DecimoTerceiroVlr = provisaoDepreciacao.DecimoTerceiro.ToString(),
                        DepreciacaoVlr = provisaoDepreciacao.Depreciacao.ToString(),
                        SaldoPrejuizoVlr = provisaoDepreciacao.SaldoPrejuizo.ToString(),
                        CalcularCompesacao = provisaoDepreciacao.CalcularCompensacao,
                        Apurar = provisaoDepreciacao.Apurar
                    };
                }
                else
                {
                    trimestreViewModel.ProvisoesDepreciacoes = new ProvisoesDepreciacoesViewModel()
                    {
                        Data = (DateTime)competenciasId,
                        Empresa = (int)empresaId
                    };
                }
            }

            return trimestreViewModel;
        }

        public TrimestreViewModel CarregarVenda(DateTime? competenciasId = null, int? empresaId = null)
        {
            var trimestreViewModel = new TrimestreViewModel();

            List<Venda> vendas = _context.Vendas.ToList();
            List<ItemVenda> itensVendas = _context.ItensVendas.ToList();
            List<Produto> produtos = _context.Produtos.ToList();

            var vendasPorEmpresa = vendas.Where(v => v.DataCompetencia == competenciasId && v.EmpresaId == empresaId);

            foreach (var venda in vendasPorEmpresa)
            {
                var itensVendasViewModel = new List<ItemVendaViewModel>();

                foreach (var itemVenda in itensVendas.Where(i => i.VendaId == venda.Id))
                {
                    var produtoViewModel = new ProdutoViewModel() { Id = itemVenda.ProdutoId, Nome = itemVenda.Produto.Nome };

                    itensVendasViewModel.Add(new ItemVendaViewModel()
                    {
                        Id = itemVenda.Id,
                        VendaId = itemVenda.VendaId,
                        ProdutoId = itemVenda.ProdutoId,
                        Quantidade = itemVenda.Quantidade,
                        PrecoVlr = itemVenda.Preco.ToString(),
                        Produto = produtoViewModel
                    });
                }

                trimestreViewModel.EstoqueVendas = new VendaViewModel()
                {
                    Id = venda.Id,
                    Observacao = venda.Observacao,
                    Data = venda.DataCompetencia,
                    Empresa = venda.EmpresaId,
                    ItensVendas = itensVendasViewModel,
                    Produtos = produtos.Select(p => new ProdutoViewModel() { Id = p.Id, Nome = p.Nome }).ToList()
                };
            }

            if (trimestreViewModel.EstoqueVendas.Id == 0)
            {
                trimestreViewModel.EstoqueVendas = new VendaViewModel()
                {
                    Data = competenciasId,
                    Empresa = empresaId,
                    Produtos = produtos.Select(p => new ProdutoViewModel() { Id = p.Id, Nome = p.Nome }).ToList()
                };
            }

            return trimestreViewModel;
        }

        public IActionResult GerarArquivo()
        {
            AdicionarCompetenciaMesAtual();

            CarregarEmpresasCompetencias();
            if (ViewBag.EmpresaSeleciodaId == null || ViewBag.CompetenciasSelecionadaId == null)
            {
                ViewBag.Message = "Porfavor, selecione uma empresa e uma competencia e filtre!";
                return View("Index", CarregarCategorias());
            }

            var competenciasId = Convert.ToDateTime($"{ViewBag.CompetenciasSelecionadaId}");
            var empresaId = Convert.ToInt32($"{ViewBag.EmpresaSeleciodaId}");
            var contas = _context.LancamentosPadroes.ToList();
            var lancamentos = _context.Lancamentos.Where(l => l.DataCompetencia == competenciasId &&
                                                                 l.EmpresaId == empresaId).ToList();
            var lancamentosContabeis = new List<TextoViewModel>();

            foreach (var conta in contas)
            {
                foreach (var lancamento in lancamentos.Where(l => l.ContaId == conta.Id))
                {
                    lancamentosContabeis.Add(new TextoViewModel()
                    {
                        Data = competenciasId,
                        CodigoContaDebito = conta.ContaDebitoId,
                        CodigoContaCredito = conta.ContaCreditoId,
                        Valor = lancamento.Valor,
                        CodigoHistorico = 10,
                        ComplementoHistorico = string.Empty,
                        IniciaLote = 1,
                        CodigoMatrizFilial = empresaId,
                        CentroCustoDebito = 1,
                        CentroCustoCredito = 1,
                    });
                }
            }

            var conteudoArquivo = string.Empty;

            foreach (var lancamento in lancamentosContabeis)
            {
                conteudoArquivo += $"{lancamento.Data.Value.ToShortDateString()};{lancamento.CodigoContaDebito};{lancamento.CodigoContaCredito};" +
                    $"{lancamento.Valor};{lancamento.CodigoHistorico};{lancamento.ComplementoHistorico};" +
                    $"{lancamento.IniciaLote};{lancamento.CodigoMatrizFilial};{lancamento.CentroCustoDebito};" +
                    $"{lancamento.CentroCustoCredito};{Environment.NewLine}";
            }

            //var conteudoArquivo = _lancamentoDomainService.GerarArquivo(empresaId, competenciasId);

            var stream = new MemoryStream(Encoding.ASCII.GetBytes(conteudoArquivo));
            return new FileStreamResult(stream, "text/plain")
            {
                FileDownloadName = "test.txt"
            };
        }
    }
}