using Domain;
using Domain.Enum;
using DomainService.Repository;
using DomainService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService
{
    public class LancamentoDomainService
    {
        private readonly IContaContabilRepository _contaContabilRepository;
        private readonly ILancamentoPadraoRepository _lancamentoPadraoRepository;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IOfxLancamentoRepository _ofxLancamentoRepository;
        private readonly IOfxContaCorrenteRepository _ofxContaCorrenteRepository;
        private readonly ICompetenciaRepository _competenciaRepositoryRepository;
        private readonly IOfxBancoRepository _ofxBancoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IAutoDescricaoRepository _autoDescricaoRepository;
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly ISaldoMensalRepository _saldoMensalRepository;
        private readonly IProvisoesDepreciacaoRepository _provisoesDepreciacaoRepository;
        private readonly IVendaRepository _vendaRepository;
        private readonly IItemVendaRepository _ItemVendaRepository;
        private readonly IProdutoRepository _produtoRepository;



        public LancamentoDomainService(
           IContaContabilRepository contaContabilRepository,
           ILancamentoPadraoRepository lancamentoPadraoRepository,
           IEmpresaRepository empresaRepository,
           IOfxLancamentoRepository ofxLancamentoRepository,
           IOfxContaCorrenteRepository ofxContaCorrenteRepository,
           IOfxBancoRepository ofxBancoRepository,
           ICompetenciaRepository competenciaRepositoryRepository,
           ICategoriaRepository categoriaRepository,
           IAutoDescricaoRepository autoDescricaoRepository,
           ILancamentoRepository lancamentoRepository,
           ISaldoMensalRepository saldoMensalRepository,
           IProvisoesDepreciacaoRepository provisoesDepreciacaoRepository,
           IVendaRepository vendaRepository,
           IItemVendaRepository ItemVendaRepository,
           IProdutoRepository produtoRepository

           )
        {
            _contaContabilRepository = contaContabilRepository;
            _lancamentoPadraoRepository = lancamentoPadraoRepository;
            _empresaRepository = empresaRepository;
            _ofxLancamentoRepository = ofxLancamentoRepository;
            _ofxContaCorrenteRepository = ofxContaCorrenteRepository;
            _competenciaRepositoryRepository = competenciaRepositoryRepository;
            _categoriaRepository = categoriaRepository;
            _autoDescricaoRepository = autoDescricaoRepository;
            _lancamentoRepository = lancamentoRepository;
            _saldoMensalRepository = saldoMensalRepository;
            _ofxBancoRepository = ofxBancoRepository;
            _provisoesDepreciacaoRepository = provisoesDepreciacaoRepository;
            _vendaRepository = vendaRepository;
            _ItemVendaRepository = ItemVendaRepository;
            _produtoRepository = produtoRepository;

        }

        public async void AdicionarCompetenciaMesAtual()
        {
            try
            {
                DateTime competenciaAtual = new(DateTime.Now.Year, DateTime.Now.Month, 01);
                var competenciaValid = _competenciaRepositoryRepository.validateCompetencia(competenciaAtual);
                if (competenciaValid)
                {
                    return;
                }

                var competencia = new Competencia()
                {
                    Data = competenciaAtual
                };
                await _competenciaRepositoryRepository.Adicionar(competencia);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }

        }

        public List<Empresa> CarregarEmpresas()
        {
            try
            {
                List<Empresa> empresas = _empresaRepository.GetAll();
                return empresas;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public List<Competencia> CarregarCompetencias()
        {
            try
            {
                List<Competencia> competencias = _competenciaRepositoryRepository.GetAll();
                return competencias;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }

        }

        public async Task<TrimestreViewModel> CarregarCategorias(int? empresasId = null, DateTime? competenciasId = null)
        {
            try
            {
                var trimestreViewModel = new TrimestreViewModel();

                var contas = _lancamentoPadraoRepository.GetAll();
                var categorias = _categoriaRepository.GetAll();
                var contasCorrentes = _ofxContaCorrenteRepository.GetByEmpresaId(empresasId);
                var autoDescricao = _autoDescricaoRepository.GetAll();


                var lancamentos = new List<Lancamento>();

                if (empresasId.HasValue && competenciasId.HasValue)
                {
                    lancamentos = _lancamentoRepository.GetByEmpresaIdDataCompetencia(empresasId, competenciasId);
                }

                foreach (var categoria in categorias)
                {
                    var contasViewModel = new List<ContaViewModel>();

                    foreach (var conta in contas.Where(c => c.CategoriaId == categoria.Id))
                    {
                        var lancamentosViewModel = new List<LancamentoViewModel>();
                        double valor = 0;
                        foreach (var contaCorrente in contasCorrentes)
                        {
                            var ofxLancamentos = _ofxLancamentoRepository.GetByContaCorrenteId(contaCorrente.Id);

                            if (conta.Codigo == 200)
                            {
                                var saldoBanco = (_saldoMensalRepository.GetByCompetenciaIdContaCorrenteId(competenciasId, contaCorrente.Id));

                                lancamentosViewModel.Add(new LancamentoViewModel()
                                {
                                    Valor = saldoBanco.Saldo,
                                    Descricao = (await _ofxBancoRepository.GetById(contaCorrente.BancoOfxId)).Nome
                                });
                            }
                            foreach (var ofxLancamento in ofxLancamentos
                               .Where(l => l.Data.Year == competenciasId.Value.Year
                                       && l.Data.Month == competenciasId.Value.Month))
                            {
                                var contaCodigo = autoDescricao.FirstOrDefault(a => a.Descricao == ofxLancamento.Descricao).LancamentoPadraoId;
                                if (ofxLancamento != null && contaCodigo == conta.Id && contaCodigo != 200)
                                {
                                    valor += ofxLancamento.ValorOfx;
                                }
                            }
                        }
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
                            Lancamentos = lancamentosViewModel
                        });

                    }

                    trimestreViewModel.Categorias.Add(new CategoriaViewModel()
                    {
                        Descricao = categoria.Descricao,
                        Contas = contasViewModel
                    });
                }
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
            catch (Exception e)
            {
                throw e.InnerException;
            }

        }

        public TrimestreViewModel CarregarTrimestre(DateTime? competenciasId = null, int? empresaId = null)
        {
            try
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
            catch (Exception e)
            {
                throw e.InnerException;
            }

        }
        public TrimestreViewModel SomarTrimestre(int[] trimestre, int? empresaId, DateTime? competenciasId = null)
        {
            try
            {
                var trimestreViewModel = new TrimestreViewModel();

                var contasCorrentes = _ofxContaCorrenteRepository.GetByEmpresaId(empresaId).ToList();
                var OfxLancamentos = _ofxLancamentoRepository.GetAll();
                List<LancamentoPadrao> contas = _lancamentoPadraoRepository.GetAll();
                List<ProvisoesDepreciacao> provisoes = _provisoesDepreciacaoRepository.GetAll();

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

                    var provisaoDepreciacao = provisoes.FirstOrDefault(p => p.EmpresaId == empresaId && p.DataCompetencia.Month == competencia);

                    if (provisaoDepreciacao != null)
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
            catch (Exception e)
            {
                throw e.InnerException;
            }

        }

        public TrimestreViewModel CarregarVenda(DateTime? competenciasId = null, int? empresaId = null)
        {
            try
            {
                var trimestreViewModel = new TrimestreViewModel();

                List<Venda> vendas = _vendaRepository.GetAll();
                List<ItemVenda> itensVendas = _ItemVendaRepository.GetAll();
                List<Produto> produtos = _produtoRepository.GetAll();

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
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public async Task<Lancamento> Salvar(DateTime competencia, TrimestreViewModel trimestreViewModel)
        {
            var lancamentoCompetencia = _lancamentoRepository.GetByDataCompetencia(competencia);
            var estoqueVendas = trimestreViewModel.EstoqueVendas;

            if (lancamentoCompetencia == false)
            {
                var insertEstoqueVendas = new Venda()
                {
                    DataCompetencia = (DateTime)estoqueVendas.Data,
                    EmpresaId = (int)estoqueVendas.Empresa,
                    Observacao = estoqueVendas.Observacao
                };

                await _vendaRepository.Adicionar(insertEstoqueVendas);

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

                        await _ItemVendaRepository.Adicionar(insertItemVenda);

                    }
                    else
                    {
                        var updateItemVenda = await _ItemVendaRepository.GetById(Convert.ToInt32(itemVenda.Id));

                        updateItemVenda.Quantidade = itemVenda.Quantidade;
                        updateItemVenda.Preco = itemVenda.Preco;

                        await _ItemVendaRepository.Editar(updateItemVenda);
                    }
                }
            }
            else
            {
                var updateEstoqueVendas =await _vendaRepository.GetById(Convert.ToInt32(estoqueVendas.Id));

                updateEstoqueVendas.DataCompetencia = (DateTime)estoqueVendas.Data;
                updateEstoqueVendas.EmpresaId = (int)estoqueVendas.Empresa;
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

                        await _ItemVendaRepository.Adicionar(insertItemVenda);
                    }
                    else
                    {
                        var updateItemVenda = await _ItemVendaRepository.GetById(Convert.ToInt32(itemVenda.Id));

                        updateItemVenda.Quantidade = itemVenda.Quantidade;
                        updateItemVenda.Preco = itemVenda.Preco;

                        await _ItemVendaRepository.Editar(updateItemVenda);
                    }
                }
            }

            var provisoesDepreciacoes = trimestreViewModel.ProvisoesDepreciacoes;

            if (lancamentoCompetencia == false)
            {
                var insertProvisoes = new ProvisoesDepreciacao()
                {
                    DataCompetencia = provisoesDepreciacoes.Data,
                    EmpresaId = provisoesDepreciacoes.Empresa,
                    DecimoTerceiro = provisoesDepreciacoes.DecimoTerceiro,
                    Ferias = provisoesDepreciacoes.Ferias,
                    Depreciacao = provisoesDepreciacoes.Depreciacao,
                    SaldoPrejuizo = provisoesDepreciacoes.SaldoPrejuizo,
                    CalcularCompensacao = provisoesDepreciacoes.CalcularCompesacao,
                    Apurar = provisoesDepreciacoes.Apurar
                };

                await _provisoesDepreciacaoRepository.Adicionar(insertProvisoes);
            }
            else
            {
                var updateProvisoes =await _provisoesDepreciacaoRepository.GetById(provisoesDepreciacoes.Id);

                updateProvisoes.DataCompetencia = provisoesDepreciacoes.Data;
                updateProvisoes.EmpresaId = provisoesDepreciacoes.Empresa;
                updateProvisoes.DecimoTerceiro = provisoesDepreciacoes.DecimoTerceiro;
                updateProvisoes.Ferias = provisoesDepreciacoes.Ferias;
                updateProvisoes.Depreciacao = provisoesDepreciacoes.Depreciacao;
                updateProvisoes.SaldoPrejuizo = provisoesDepreciacoes.SaldoPrejuizo;
                updateProvisoes.CalcularCompensacao = provisoesDepreciacoes.CalcularCompesacao;
                updateProvisoes.Apurar = provisoesDepreciacoes.Apurar;

                await _provisoesDepreciacaoRepository.Editar(updateProvisoes);
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

                if (lancamento.Id != 0 && lancamento.Valor == 0)
                {
                    await _lancamentoRepository.Deletar(lancamento.Id);
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

                    await _lancamentoRepository.Adicionar(insertLancamento);
                }
                else
                {
                    var updateLancamento = await _lancamentoRepository.GetById(lancamento.Id);

                    updateLancamento.Descricao = lancamento.Descricao;
                    updateLancamento.Valor = lancamento.Valor;

                    await _lancamentoRepository.Editar(updateLancamento);
                }
            }
            var primeiroLancamento = lancamentos.FirstOrDefault();

            return primeiroLancamento;

        }

        public string GerarArquivo(int? empresaId = null, DateTime? competenciasId = null)
        {
            try
            {
                var contas = _lancamentoPadraoRepository.GetAll();
                var lancamentos = _lancamentoRepository.GetByEmpresaIdDataCompetencia(empresaId, competenciasId);

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

                return conteudoArquivo;     
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }

    }
}
