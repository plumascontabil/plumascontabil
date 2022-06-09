using Domain;
using DomainService.Repository;
using DomainService.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DomainService
{
    public class OfxImportacoesDomainService
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



        public OfxImportacoesDomainService(
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
        //public async Task<ExtratoBancarioViewModel> OfxImportar(IFormFile ofxArquivo = null, String caminhoDestinoArquivo)
        //{
        //    try
        //    {
        //        //Listas
        //        var empresas = _context.Empresas.ToList();
        //        var contasContabeis = _context.ContasContabeis.ToList();
        //        var lancamentosPadroes = _context.LancamentosPadroes.ToList();
        //        var autoDescricoes = _context.AutoDescricoes;

        //        //Views Models
        //        var lancamentoOfxViewModel = new List<OfxLancamentoViewModel>();
        //        var contaCorrenteViewModel = new OfxContaCorrenteViewModel();
        //        var extratoBancarioViewModel = new ExtratoBancarioViewModel();
        //        var saldoMensalViewModel = new SaldoMensalViewModel();

        //        decimal saldo = 0;
        //        //If Leitura arquivo não nulo
        //        if (ofxArquivo != null)
        //        {
        //            using (var stream = new FileStream(caminhoDestinoArquivo, FileMode.Create))
        //            {
        //                await ofxArquivo.CopyToAsync(stream);
        //            }
        //            //Extraindo conteudo do arquivo em um objeto do tipo Extract
        //            Extract extratoBancario = Parser.GenerateExtract(caminhoDestinoArquivo);
        //            if (extratoBancario != null)
        //            {
        //                var documento = new OFXDocumentParser();
        //                var dadoDocumento = documento.Import(new FileStream(caminhoDestinoArquivo, FileMode.Open));
        //                saldo = dadoDocumento.Balance.LedgerBalance;
        //            }

        //            saldoMensalViewModel = new SaldoMensalViewModel()
        //            {
        //                SaldoMensal = saldo,
        //                Competencia = extratoBancario.InitialDate,
        //                ContaCorrenteId = _context.ContasCorrentes
        //                    .FirstOrDefault(c => c.NumeroConta == extratoBancario.BankAccount.AccountCode).Id,
        //            };

        //            //varrendo arquivo e adicionado as ViewsModel
        //            foreach (var dados in extratoBancario.Transactions)
        //            //foreach (var dados in extratoBancario.Transactions)
        //            {
        //                var banco = _context.OfxBancos
        //                    .FirstOrDefault(b => b.Codigo == Convert.ToInt32(extratoBancario.BankAccount.Bank.Code));

        //                if (banco == null)
        //                {
        //                    ViewBag.AdicionarBanco = "Banco Inexistente";
        //                    break;
        //                }

        //                var bancoViewModel = new OfxBancoViewModel()
        //                {
        //                    Id = banco.Id,
        //                    Codigo = banco.Codigo,
        //                    Nome = banco.Nome
        //                };

        //                if (_context.OfxLancamentos.ToList().Any(c => c.Documento == dados.Id) == false)
        //                {
        //                    if (autoDescricoes.FirstOrDefault(a => a.Descricao == dados.Description) == null)
        //                    {
        //                        lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
        //                        {
        //                            Id = dados.Id,
        //                            TransationValue = dados.TransactionValue,
        //                            Description = dados.Description,
        //                            Date = dados.Date,
        //                            CheckSum = dados.Checksum,
        //                            Type = dados.Type,
        //                            SaldoMensal = saldoMensalViewModel,
        //                            LancamentosPadroes = ConstruirLancamentosPadroesSelectList(lancamentosPadroes)
        //                        });
        //                    }
        //                    else
        //                    {
        //                        lancamentoOfxViewModel.Add(new OfxLancamentoViewModel()
        //                        {
        //                            Id = dados.Id,
        //                            TransationValue = dados.TransactionValue,
        //                            Description = dados.Description,
        //                            Date = dados.Date,
        //                            CheckSum = dados.Checksum,
        //                            Type = dados.Type,
        //                            SaldoMensal = saldoMensalViewModel,
        //                            LancamentosPadroes = ConstruirLancamentosPadroesSelectList(lancamentosPadroes),
        //                            LancamentoPadraoSelecionado = autoDescricoes.FirstOrDefault(a => a.Descricao == dados.Description).LancamentoPadraoId
        //                        });
        //                    }

        //                    contaCorrenteViewModel = new OfxContaCorrenteViewModel()
        //                    {
        //                        OfxLancamentos = lancamentoOfxViewModel,
        //                        NumeroConta = extratoBancario.BankAccount.AccountCode,
        //                    };

        //                    extratoBancarioViewModel = new ExtratoBancarioViewModel()
        //                    {
        //                        Empresas = ConstruirEmpresas(empresas),
        //                        ContasCorrentes = contaCorrenteViewModel,
        //                        Banco = bancoViewModel
        //                    };
        //                }
        //                else
        //                {
        //                    break;
        //                }
        //            }
        //            //Deletando arquivo do servidor
        //            System.IO.File.Delete(caminhoDestinoArquivo);
        //            System.IO.File.Delete($"{caminhoDestinoArquivo}.xml");
        //        }

        //        return extratoBancarioViewModel;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e.InnerException;
        //    }
        //}

        private static SelectList ConstruirLancamentosPadroesSelectList(IEnumerable<LancamentoPadrao> lancamentoPadroes)
          => new(lancamentoPadroes.Select(c => new { c.Codigo, Descricao = $"{c.Codigo} - {c.Descricao}" }), "Codigo", "Descricao");
        private static SelectList ConstruirEmpresas(IEnumerable<Empresa> empresas)
            => new(empresas.Select(e => new { e.Codigo, Razao = $"{e.Codigo} - {e.RazaoSocial}" }), "Codigo", "Razao");

        public async Task<ExtratoBancarioViewModel> OfxReimportar(ExtratoBancarioViewModel extratoViewModel = null)
        {
            try
            {
                //Listas
                var empresas = _empresaRepository.GetAll();
                var contasContabeis = _contaContabilRepository.GetAll();
                var lancamentosPadroes = _lancamentoPadraoRepository.GetAll();
                //var autoDescricoes = _context.AutoDescricoes;

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
                            LancamentoPadraoSelecionado = Convert.ToInt32(_lancamentoPadraoRepository.GetByCodigoId(lancamentoPadrao.Codigo).Codigo)

                        });
                    }
                    var banco = await _ofxBancoRepository.GetByCodigoId(extratoViewModel.Banco.Codigo);

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

                return extratoBancarioViewModel;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }


        public async void OfxSalvar(ExtratoBancarioViewModel dados)
        {
            try
            {
                decimal saldoMensalTotal = 0;
                foreach (var dado in dados.ContasCorrentes.OfxLancamentos)
                {
                    var cc = _ofxContaCorrenteRepository.GetByNumeroContaExists(dados.ContasCorrentes.NumeroConta);
                    var banco = _ofxBancoRepository.GetByCodigoIdExists(dados.Banco.Codigo);
                    var lancamentoPadrao = _lancamentoPadraoRepository.GetByCodigoId(dado.LancamentoPadraoSelecionado);
                    if (banco == false)
                    {
                        //banco não cadastrado
                    }

                    if (cc == false)
                    {
                        //Todo: adicionar Conta corrente
                        var contaCorrenteAdd = new OfxContaCorrente()
                        {
                            NumeroConta = dados.ContasCorrentes.NumeroConta,
                            EmpresaId = (await _empresaRepository.GetById(dados.EmpresaSelecionada)).Codigo,
                            BancoOfxId = (await _ofxBancoRepository.GetByCodigoId(dados.Banco.Codigo)).Id
                        };
                        await _ofxContaCorrenteRepository.Adicionar(contaCorrenteAdd);;
                    }


                    var contaCorrente = _ofxContaCorrenteRepository.GetByNumeroConta(dados.ContasCorrentes.NumeroConta);

                    var saldoMensalId = _saldoMensalRepository.GetByDataContaCorrenteId(Convert.ToDateTime(dado.Date.ToString("yyyy/MM")), contaCorrente.Id);

                    if (saldoMensalId == null)
                    {
                        var saldoMensalAdd = new SaldoMensal()
                        {
                            Competencia = dado.SaldoMensal.Competencia,
                            Saldo = dado.SaldoMensal.SaldoMensal,
                            ContaCorrenteId = dado.SaldoMensal.ContaCorrenteId
                        };
                        await _saldoMensalRepository.Adicionar(saldoMensalAdd);                      
                    }

                    var ofxLancamento = new OfxLancamento()
                    {
                        Documento = dado.Id,
                        TipoLancamento = dado.Type,
                        Descricao = dado.Description,
                        ValorOfx = dado.TransationValue,
                        Data = dado.Date,
                        ContaCorrenteId = (_ofxContaCorrenteRepository.GetByNumeroConta(dados.ContasCorrentes.NumeroConta)).Id,
                        LancamentoPadraoId = (_lancamentoPadraoRepository.GetByCodigoId(dado.LancamentoPadraoSelecionado)).Id
                    };
                    await _ofxLancamentoRepository.Adicionar(ofxLancamento);

                    var descricao = _autoDescricaoRepository.GetByDescricao(dado.Description);

                    if (descricao == null)
                    {
                        var autoDescricaoAdd = new AutoDescricao()
                        {
                            Descricao = dado.Description,
                            LancamentoPadraoId = Convert.ToInt32(lancamentoPadrao.Id)
                        };
                        await _autoDescricaoRepository.Adicionar(autoDescricaoAdd);
                        
                    }
                    else if (descricao.LancamentoPadraoId == lancamentoPadrao.Id
                        && descricao.Descricao != dado.Description)
                    {
                        var autoDescricaoAdd = new AutoDescricao()
                        {
                            Descricao = dado.Description,
                            LancamentoPadraoId = lancamentoPadrao.Id
                        };
                        await _autoDescricaoRepository.Adicionar(autoDescricaoAdd);

                    }
                    else
                    {
                        continue;
                    }
                }

            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }


    }
}
