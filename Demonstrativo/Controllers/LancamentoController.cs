using Demonstrativo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace Demonstrativo.Controllers
{ 
    [Authorize]
    public class LancamentoController : Controller
    {
        Context _context;

        public LancamentoController(Context context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            AdicionarCompetenciaMesAtual();

            CarregarEmpresasCompetencias();

            return View(CarregarCategorias());
        }

        private void AdicionarCompetenciaMesAtual()
        {
            DateTime competenciaAtual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);

            if(_context.Competencias.Any(c => c.Data == competenciaAtual))
            {
                return;
            }

            Competencia competencia = new Competencia()
            {
                Data = competenciaAtual
            };

            _context.Competencias.Add(competencia);
            _context.SaveChanges();
        }

        private void CarregarEmpresasCompetencias(int? empresaId = null, DateTime? competenciasId = null)
        {
            List<Empresa> empresas = _context.Empresas.ToList();
            List<Competencia> competencias = _context.Competencias.ToList();

            ViewBag.CompetenciasId = new SelectList(
               competencias.Select(c => new { Value = c.Data.ToShortDateString(), Text = c.Data.ToString("MM/yyyy") })
               , "Value", "Text", competenciasId.HasValue ? competenciasId.Value.ToShortDateString() : competenciasId);

            ViewBag.EmpresaId = new SelectList(empresas, "Codigo", "RazaoSocial", empresaId);
        }

        private TrimestreViewModel CarregarCategorias(int? empresaId=null, DateTime? competenciasId=null)
        {
            var trimestreViewModel = new TrimestreViewModel();

            List<Conta> contas = _context.Contas.ToList();
            List<Categoria> categorias = _context.Categorias.ToList();

            var lancamentos = new List<Lancamento>();

            if (empresaId.HasValue && competenciasId.HasValue)
            {
                lancamentos =  _context.Lancamentos.Where(x => x.EmpresaId == empresaId && x.DataCompetencia == competenciasId)
                    .ToList();
            }

            foreach (var categoria in categorias)
            {
                var contasViewModel = new List<ContaViewModel>();

                foreach (var conta in contas.Where(c => c.CategoriaId == categoria.Id))
                {
                    var lancamentosViewModel = new List<LancamentoViewModel>();

                    foreach (var lancamento in lancamentos.Where(l => l.ContaId == conta.Id))
                    {
                        lancamentosViewModel.Add(new LancamentoViewModel()
                        {
                            Id = lancamento.Id,
                            Data = lancamento.DataCompetencia,
                            Empresa = lancamento.EmpresaId,
                            Conta = lancamento.ContaId,
                            Descricao = lancamento.Descricao,
                            //PodeDigitarDescricao = conta.Codigo == 38 || conta.Codigo == 36 || conta.Codigo == 200 || conta.Codigo == 201,
                            Valor = lancamento.Valor
                        });

                    }
                    if (conta.Codigo == 38 || conta.Codigo == 36 || conta.Codigo == 200 || conta.Codigo == 201)
                    {
                        lancamentosViewModel.Add(new LancamentoViewModel() { PodeDigitarDescricao = true, Conta = conta.Codigo});
                        lancamentosViewModel.Add(new LancamentoViewModel() { PodeDigitarDescricao = true, Conta = conta.Codigo });
                        lancamentosViewModel.Add(new LancamentoViewModel() { PodeDigitarDescricao = true, Conta = conta.Codigo });
                        lancamentosViewModel.Add(new LancamentoViewModel() { PodeDigitarDescricao = true, Conta = conta.Codigo });
                        lancamentosViewModel.Add(new LancamentoViewModel() { PodeDigitarDescricao = true, Conta = conta.Codigo });
                    }

                    else if (!lancamentosViewModel.Any())
                    {
                        lancamentosViewModel.Add(new LancamentoViewModel());
                    }
                    contasViewModel.Add(new ContaViewModel()
                    {
                        Id = conta.Id,
                        Codigo = conta.Codigo,
                        Descricao = conta.Descricao,
                        Lancamentos = lancamentosViewModel
                    });
                }

                trimestreViewModel.Categorias.Add(new CategoriaViewModel()
                {
                    Descricao = categoria.Descricao,
                    Contas = contasViewModel
                });
            }
            var trimestre = CarregarTrimestre(competenciasId, empresaId);
            var estorqueVenda = CarregarVenda(competenciasId, empresaId);

            trimestreViewModel.LancamentosCompra = trimestre.LancamentosCompra;
            trimestreViewModel.LancamentosReceita = trimestre.LancamentosReceita;
            trimestreViewModel.LancamentosDespesa = trimestre.LancamentosDespesa;
            trimestreViewModel.Trimestre = trimestre.Trimestre;
            trimestreViewModel.ProvisoesDepreciacoes = trimestre.ProvisoesDepreciacoes;
            trimestreViewModel.EstoqueVendas = estorqueVenda.EstoqueVendas;

            return trimestreViewModel;
        }

        [HttpPost]
        public IActionResult Filtrar(int empresaId, DateTime competenciasId)
        {
            CarregarEmpresasCompetencias(empresaId, competenciasId);

            ViewBag.EmpresaSeleciodaId = empresaId;
            ViewBag.CompetenciasSelecionadaId = competenciasId.ToString("yyyy-MM-dd");

            return View("Index", CarregarCategorias(empresaId, competenciasId));
        }

        [HttpPost]
        public IActionResult Salvar(TrimestreViewModel trimestreViewModel)
        {
            var estoqueVendas = trimestreViewModel.EstoqueVendas;

            if (estoqueVendas.Id == 0 )
            {
                var insertEstoqueVendas = new Venda();

                insertEstoqueVendas.DataCompetencia = (DateTime)estoqueVendas.Data;
                insertEstoqueVendas.EmpresaId = (int)estoqueVendas.Empresa;
                insertEstoqueVendas.Observacao = estoqueVendas.Observacao;

                _context.Vendas.Add(insertEstoqueVendas);
                _context.SaveChanges();
                
                foreach (var itemVenda in estoqueVendas.ItensVendas)
                {
                    var insertItemVenda = new ItemVenda();

                    if (itemVenda.Id == 0 && itemVenda.Quantidade == 0 || itemVenda.Preco == 0)
                    {
                        continue;
                    }

                    if (itemVenda.Id == 0)
                    {
                        insertItemVenda.VendaId = insertEstoqueVendas.Id;
                        insertItemVenda.ProdutoId = itemVenda.ProdutoId;
                        insertItemVenda.Quantidade = itemVenda.Quantidade;
                        insertItemVenda.Preco = itemVenda.Preco;

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

                updateEstoqueVendas.DataCompetencia = (DateTime)estoqueVendas.Data;
                updateEstoqueVendas.EmpresaId = (int)estoqueVendas.Empresa;
                updateEstoqueVendas.Observacao = estoqueVendas.Observacao;
                
                foreach (var itemVenda in estoqueVendas.ItensVendas)
                {
                    if (itemVenda.Id == 0 && itemVenda.Quantidade == 0 || itemVenda.Preco == 0)
                    {
                        continue;
                    }

                    var insertItemVenda = new ItemVenda();
                    if (itemVenda.Id == 0)
                    {
                        insertItemVenda.VendaId = updateEstoqueVendas.Id;
                        insertItemVenda.ProdutoId = itemVenda.ProdutoId;
                        insertItemVenda.Quantidade = itemVenda.Quantidade;
                        insertItemVenda.Preco = itemVenda.Preco;

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
            
            if (provisoesDepreciacoes.Id == 0 )
            {
                var insertProvisoes = new ProvisoesDepreciacao();

                insertProvisoes.DataCompetencia = provisoesDepreciacoes.Data;
                insertProvisoes.EmpresaId = provisoesDepreciacoes.Empresa;
                insertProvisoes.DecimoTerceiro = provisoesDepreciacoes.DecimoTerceiro;
                insertProvisoes.Ferias = provisoesDepreciacoes.Ferias;
                insertProvisoes.Depreciacao = provisoesDepreciacoes.Depreciacao;
                insertProvisoes.SaldoPrejuizo = provisoesDepreciacoes.SaldoPrejuizo;
                insertProvisoes.CalcularCompensacao = provisoesDepreciacoes.CalcularCompesacao;
                insertProvisoes.Apurar = provisoesDepreciacoes.Apurar;

                _context.ProvisoesDepreciacoes.Add(insertProvisoes);
                _context.SaveChanges();
            }
            else
            {
                var updateProvisoes = _context.ProvisoesDepreciacoes.Find(provisoesDepreciacoes.Id);

                updateProvisoes.DataCompetencia = provisoesDepreciacoes.Data;
                updateProvisoes.EmpresaId = provisoesDepreciacoes.Empresa;
                updateProvisoes.DecimoTerceiro = provisoesDepreciacoes.DecimoTerceiro;
                updateProvisoes.Ferias = provisoesDepreciacoes.Ferias;
                updateProvisoes.Depreciacao = provisoesDepreciacoes.Depreciacao;
                updateProvisoes.SaldoPrejuizo = provisoesDepreciacoes.SaldoPrejuizo;
                updateProvisoes.CalcularCompensacao = provisoesDepreciacoes.CalcularCompesacao;
                updateProvisoes.Apurar = provisoesDepreciacoes.Apurar;

                _context.ProvisoesDepreciacoes.Update(updateProvisoes);
                _context.SaveChanges();
            }

            var lancamentosViewModel = trimestreViewModel.Categorias.SelectMany(x => x.Contas.SelectMany(x => x.Lancamentos));
           
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

                if(lancamento.Id != 0 && lancamento.Valor == 0)
                {
                    _context.Lancamentos.Remove(lancamento);
                    _context.SaveChanges();
                    continue;
                }

                if (lancamento.Id == 0)
                {
                    var insertLancamento = new Lancamento();
                    if(lancamento.Descricao == null || lancamento.ContaId == 156 || lancamento.ContaId == 98 || lancamento.ContaId == 157 || lancamento.ContaId == 140)
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

            if (primeiroLancamento != null)
                return Filtrar(primeiroLancamento.EmpresaId,primeiroLancamento.DataCompetencia);
            else
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
                int[] trimestre = {1,2,3 };
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
        
        public TrimestreViewModel SomarTrimestre(int[]? trimestre, int? empresaId, DateTime? competenciasId = null)
        {
            var trimestreViewModel = new TrimestreViewModel();

            List<Lancamento> lancamentos = _context.Lancamentos.Include(x => x.Conta).ToList();

            List<Conta> contas = _context.Contas.ToList();
            List<ProvisoesDepreciacao> provisoes = _context.ProvisoesDepreciacoes.ToList();

            trimestreViewModel.Trimestre = trimestre;

            foreach (var competencia in trimestre)
            {
                foreach (var lancamento in lancamentos.Where(l => l.EmpresaId == empresaId && l.DataCompetencia.Month == competencia  && l.Conta?.TipoContaId == (int)ETipoConta.Compras))
                {
                    trimestreViewModel.LancamentosCompra.Add(new LancamentoViewModel()
                    {
                        Id = lancamento.Id,
                        Data = lancamento.DataCompetencia,
                        Empresa = lancamento.EmpresaId,
                        Conta = lancamento.ContaId,
                        Descricao = lancamento.Descricao,
                        Valor = lancamento.Valor
                    });
                }

                foreach (var lancamento in lancamentos.Where(l => l.EmpresaId == empresaId && l.DataCompetencia.Month == competencia && l.Conta?.TipoContaId == (int)ETipoConta.EstoqueInicial))
                {
                    trimestreViewModel.LancamentosCompra.Add(new LancamentoViewModel()
                    {
                        Id = lancamento.Id,
                        Data = lancamento.DataCompetencia,
                        Empresa = lancamento.EmpresaId,
                        Conta = lancamento.ContaId,
                        Descricao = lancamento.Descricao,
                        Valor = lancamento.Valor
                    });
                }

                foreach (var lancamento in lancamentos.Where(l => l.EmpresaId == empresaId && l.DataCompetencia.Month == competencia && l.Conta?.TipoContaId == (int)ETipoConta.EstoqueFinal))
                {
                    trimestreViewModel.LancamentosCompra.Add(new LancamentoViewModel()
                    {
                        Id = lancamento.Id,
                        Data = lancamento.DataCompetencia,
                        Empresa = lancamento.EmpresaId,
                        Conta = lancamento.ContaId,
                        Descricao = lancamento.Descricao,
                        Valor = lancamento.Valor
                    });
                }

                foreach (var lancamento in lancamentos.Where(l => l.EmpresaId == empresaId && l.DataCompetencia.Month == competencia && l.Conta?.TipoContaId == (int)ETipoConta.Receitas))
                {
                    trimestreViewModel.LancamentosReceita.Add(new LancamentoViewModel()
                    {
                        Id = lancamento.Id,
                        Data = lancamento.DataCompetencia,
                        Empresa = lancamento.EmpresaId,
                        Conta = lancamento.ContaId,
                        Descricao = lancamento.Descricao,
                        Valor = lancamento.Valor
                    });
                }
                
                foreach (var conta in contas.Where(c => c.TipoContaId == (int)ETipoConta.Despesas))
                {
                    if(conta.Lancamentos == null)
                    {
                        continue;
                    }

                    foreach (var lancamento in conta.Lancamentos.Where(l => l.EmpresaId == empresaId && l.DataCompetencia.Month == competencia))
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

                var provisaoDepreciacao = provisoes.FirstOrDefault(p => p.EmpresaId == empresaId && p.DataCompetencia.Month == competencia);

                if(provisaoDepreciacao != null)
                {
                    trimestreViewModel.ProvisoesDepreciacoes = new ProvisoesDepreciacoesViewModel()
                    {
                        Id = provisaoDepreciacao.Id,
                        Data = provisaoDepreciacao.DataCompetencia,
                        Empresa = provisaoDepreciacao.EmpresaId,
                        Ferias = provisaoDepreciacao.Ferias,
                        DecimoTerceiro = provisaoDepreciacao.DecimoTerceiro,
                        Depreciacao = provisaoDepreciacao.Depreciacao,
                        SaldoPrejuizo = provisaoDepreciacao.SaldoPrejuizo,
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
                List<ItemVendaViewModel> itensVendasViewModel = new List<ItemVendaViewModel>();                

                foreach (var itemVenda in itensVendas.Where(i => i.VendaId == venda.Id))
                {
                    var produtoViewModel = new ProdutoViewModel() { Id = itemVenda.ProdutoId, Nome = itemVenda.Produto.Nome };

                    itensVendasViewModel.Add(new ItemVendaViewModel()
                    {
                        Id = itemVenda.Id,
                        VendaId = itemVenda.VendaId,
                        ProdutoId = itemVenda.ProdutoId,
                        Quantidade = itemVenda.Quantidade,
                        Preco = itemVenda.Preco,
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
                    Produtos = produtos.Select(p => new ProdutoViewModel() { Id = p.Id,Nome = p.Nome}).ToList()
                };
            }
            
            if(trimestreViewModel.EstoqueVendas.Id == 0)
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
        
        public IActionResult GerarArquivo(int? empresaId = null, DateTime? competenciasId = null)
        {
            var contas = _context.Contas.ToList();
            var lancamentos = _context.Lancamentos.Where(l => l.DataCompetencia == competenciasId &&
                                                                 l.EmpresaId == empresaId).ToList();
            var lancamentosContabeis = new List<TextoViewModel>();

            foreach(var conta in contas)
            {
                foreach(var lancamento in lancamentos.Where(l => l.ContaId == conta.Id))
                {
                    lancamentosContabeis.Add(new TextoViewModel()
                    {
                        Data = competenciasId,
                        CodigoContaDebito = conta.LancamentoDebito,
                        CodigoContaCredito = conta.LancamentoCredito,
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

            var stream = new MemoryStream(Encoding.ASCII.GetBytes(conteudoArquivo));
            return new FileStreamResult(stream, "text/plain")
            {
                FileDownloadName = "test.txt"
            };
        }
    }
}