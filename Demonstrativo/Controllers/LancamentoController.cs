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
using System.Security.Claims;
using System.Globalization;

namespace Demonstrativo.Controllers
{
    [Authorize]
    public class LancamentoController : BaseController
    {
        readonly Context _context;
        private readonly ILogger<Lancamento> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        //private readonly LancamentoDomainService _lancamentoDomainService;

        public LancamentoController(Context context,
            ILogger<Lancamento> logger,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager) : base(context, roleManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            //_lancamentoDomainService = LancamentoDomainService;
        }
        public IActionResult Dre()
        {
            ViewBag.Consultor = _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)).Result;
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

            var receitas = new List<ReceitasViewModel>();
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


            var lote = new List<OfxLoteLancamento>();

            if (empresasId.HasValue && competenciasId.HasValue)
            {
                lote = _context.OfxLoteLancamento.Include(x => x.Lancamentos)
                    .ThenInclude(x => x.ContaCorrente)
                    .Include(x => x.Lancamentos)
                    .ThenInclude(x => x.LancamentoPadrao)
                    .Where(x => x.EmpresaId == empresasId && x.CompetenciaId == competenciasId)
                    .ToList();
            }
            var contasCorrentesLancamentos = new List<OfxLancamento>();
            var lancamentos = new List<Lancamento>();

            if (empresasId.HasValue)
            {
                var ids = contasCorrentes.Select(x => x.Id).ToList();

                lote.ForEach(el =>
                {
                    contasCorrentesLancamentos = contasCorrentesLancamentos.Concat(el.Lancamentos.Where(o => ids.Any(x => x == o.ContaCorrenteId) == true).ToList()).ToList();

                });
                contasCorrentes = contasCorrentesLancamentos.Select(x => x.ContaCorrente).Distinct().ToList();

                lancamentos = _context.Lancamentos.Where(f => f.EmpresaId == empresasId && f.DataCompetencia == competenciasId).ToList();

                //  _context.OfxLancamentos.Where(o => ids.Any(x => x == o.ContaCorrenteId) == true).ToList();
                // Retirar os inativar
                contasCorrentesLancamentos = contasCorrentesLancamentos.Where(f => !f.Inativar.HasValue || !f.Inativar.Value).ToList();

            }

            var lancamentosViewModelBancos = new List<LancamentoViewModel>();
            contasCorrentes.ForEach(contaCorrente =>
            {
                var banco = _context.OfxBancos.FirstOrDefault(c => c.Id == contaCorrente.BancoOfxId);
                var saldoBanco = _context.SaldoMensal.FirstOrDefault(c => c.Competencia == competenciasId && c.ContaCorrenteId == contaCorrente.Id);
                var conta = string.IsNullOrEmpty(banco.CodigoContabil) ? contas.FirstOrDefault(f => f.Codigo == 200) : contas.FirstOrDefault(f => f.Codigo == Convert.ToInt32(banco.CodigoContabil));


                if (conta == null)
                {
                    conta = contas.FirstOrDefault(f => f.Codigo == 200);
                }
                lancamentosViewModelBancos.Add(new LancamentoViewModel()
                {
                    ValorStr = (saldoBanco?.Saldo ?? 0).ToString(),//contasCorrentesLancamentos.Where(f => f.ContaCorrenteId == contaCorrente.Id).Sum(x => x.ValorOfx).ToString(),
                    Descricao = $"{banco.Codigo} - {banco.Nome}  Ag.: {contaCorrente.NumeroAgencia} C/c.: {contaCorrente.NumeroConta}",
                    Conta = conta.Codigo

                });


            });


            categorias.ForEach(categoria =>
            {
                var contasViewModel = new List<ContaViewModel>();
                contas.Where(c => c.CategoriaId == categoria.Id).ToList().ForEach(conta =>
                {

                    decimal valor = 0;

                    var lancamentosViewModel = new List<LancamentoViewModel>();
                    //var ofxLancamentos = contasCorrentesLancamentos.Where(f => f.ContaCorrenteId == contaCorrente.Id).ToList();

                    var ofxLanc = contasCorrentesLancamentos.Where(l => l.LancamentoPadraoId.HasValue).Where(l => (int)conta.Id == (int)l.LancamentoPadraoId.Value).ToList();

                    ofxLanc.ForEach(ofxLancamento =>
                              {
                                  //var contaCodigo = autoDescricao.FirstOrDefault(a => a.Descricao == ofxLancamento.Descricao).LancamentoPadraoId;
                                  if (ofxLancamento != null)
                                  {
                                      valor += ofxLancamento.ValorOfx;
                                  }
                              });

                    var lancManual = lancamentos.Where(f => f.ContaId.HasValue).Where(l => (int)conta.Id == (int)l.ContaId).ToList();


                    lancManual.ForEach(ofxLancamento =>
                    {
                        //var contaCodigo = autoDescricao.FirstOrDefault(a => a.Descricao == ofxLancamento.Descricao).LancamentoPadraoId;
                        if (ofxLancamento != null)
                        {
                            valor += ofxLancamento.Valor;
                        }
                    });

                    if (valor != 0)
                    {
                        var lanc = new LancamentoViewModel()
                        {
                            ValorStr = Convert.ToString(conta.TipoLancamento == "C" ? Convert.ToDecimal(valor) * -1 : Convert.ToDecimal(valor))
                        };
                        if ((categoria.Descricao.ToUpper() == "Receitas".ToUpper()))
                        {
                            lanc.ValorStr = Convert.ToString(valor);
                        }
                        lancamentosViewModel.Add(lanc);

                    }


                    if (!lancamentosViewModel.Any())
                    {
                        lancamentosViewModel.Add(new LancamentoViewModel());
                    }
                    if (conta.Codigo != 200 && !categoria.Descricao.ToUpper().Equals("Saldo Final em Bancos".ToUpper()))
                    {
                        contasViewModel.Add(new ContaViewModel()
                        {
                            Id = conta.Id,
                            Codigo = conta.Codigo,
                            Descricao = conta.Descricao,
                            TipoLancamento = conta.TipoLancamento,
                            Lancamentos = lancamentosViewModel
                        });
                    }
                    else
                    {
                        if (lancamentosViewModelBancos.Count > 0)
                        {
                            //.ForEach(f =>
                            //{

                            //});
                            //var xxx = new List<LancamentoViewModel>();
                            //xxx.Add(f);
                            contasViewModel.Add(new ContaViewModel()
                            {
                                Id = conta.Id,
                                Codigo = conta.Codigo,
                                Descricao = conta.Descricao,
                                TipoLancamento = conta.TipoLancamento,
                                Lancamentos = lancamentosViewModelBancos.Where(f => f.Conta.Value == conta.Codigo).ToList()
                            });

                        }
                        else
                        {
                            var xxx = new List<LancamentoViewModel>();
                            xxx.Add(new LancamentoViewModel()
                            {
                                Descricao = "Não Há Conta Corrente"
                            });
                            contasViewModel.Add(new ContaViewModel()
                            {
                                Id = conta.Id,
                                Codigo = conta.Codigo,
                                Descricao = conta.Descricao,
                                TipoLancamento = conta.TipoLancamento,
                                Lancamentos = xxx
                            });
                        }
                    }





                });


                if (categoria.Descricao.ToUpper() == "PROVISÕES SALÁRIOS".ToUpper())
                {
                    var idx = contasViewModel.FindIndex(f => f.Id == 182);


                    var bruto = contasViewModel.FirstOrDefault(f => f.Descricao.ToUpper() == "Salário Bruto".ToUpper());
                    var totDescontos = contasViewModel.FirstOrDefault(f => f.Descricao.ToUpper() == "TOTAL DE DESCONTOS".ToUpper());
                    decimal brutoVLR = 0;
                    decimal totDescontosVLR = 0;
                    bruto.Lancamentos.ForEach(el =>
                    {
                        brutoVLR = el != null ? el.Valor : 0;
                    });
                    totDescontos.Lancamentos.ForEach(el =>
                    {
                        totDescontosVLR = el != null ? el.Valor : 0;
                    });
                    contasViewModel[idx].Lancamentos[0].ValorStr = (brutoVLR - totDescontosVLR).ToString();
                    var total = new ContaViewModel()
                    {
                        Codigo = null,
                        Descricao = "Total despesas Folha".ToUpper(),
                        Lancamentos = new List<LancamentoViewModel>(),
                        TipoLancamento = "C"
                    };
                    //var dd = new LancamentoViewModel();
                    //dd.Id = 777;
                    //dd.ValorStr = (
                    //contasViewModel.Where(f => f.Descricao.ToUpper() == "Salário Bruto".ToUpper()
                    //|| f.Descricao.ToUpper() == "INSS(TOTAL FOLHA)".ToUpper()
                    //|| f.Descricao.ToUpper() == "FGTS".ToUpper()).Sum(x => x.Lancamentos[0].Valor)

                    //- contasViewModel.Where(f => f.Descricao.ToUpper() == "INSS - SEGURADOS".ToUpper()
                    // || f.Descricao.ToUpper() == "FÉRIAS".ToUpper()
                    // || f.Descricao.ToUpper() == "DESCONTOS / ATRASOS".ToUpper()).Sum(x => x.Lancamentos[0].Valor)

                    //).ToString();
                    //total.Lancamentos.Add(dd);
                    //contasViewModel.Add(total);

                }


                if (categoria.Descricao == "CONTAS A RECEBER")
                {
                    contasViewModel.ForEach(el =>
                    {
                        decimal valorlancado = 0;
                        decimal valorBaixado = 0;
                        var ofxLanc = contasCorrentesLancamentos.Where(l => l.LancamentoPadraoId.HasValue).Where(l => (int)el.Id == (int)l.LancamentoPadraoId.Value).ToList();


                        ofxLanc.ForEach(ofxLancamento =>
                        {
                            //var contaCodigo = autoDescricao.FirstOrDefault(a => a.Descricao == ofxLancamento.Descricao).LancamentoPadraoId;
                            if (ofxLancamento != null)
                            {
                                valorBaixado += ofxLancamento.ValorOfx;
                            }
                        });
                        valorBaixado = valorBaixado;

                        var lancManual = lancamentos.Where(f => f.ContaId.HasValue).Where(l => (int)el.Id == (int)l.ContaId).ToList();


                        lancManual.ForEach(ofxLancamento =>
                        {
                            //var contaCodigo = autoDescricao.FirstOrDefault(a => a.Descricao == ofxLancamento.Descricao).LancamentoPadraoId;
                            if (ofxLancamento != null)
                            {
                                valorlancado += ofxLancamento.Valor;
                            }
                        });
                        valorlancado = valorlancado;


                        el.Lancamentos[0].ValorStr = ((valorlancado - valorBaixado)).ToString();
                    });


                }

                if (categoria.Descricao == "CONTAS RESULTADOS")
                {
                    var compras = contasViewModel.Where(f => f.Descricao.ToUpper().Contains("COMPRA")).ToList();
                    var estoqueInicial = contasViewModel.Where(f => f.Descricao.ToUpper().Contains("INICIAL")).ToList();
                    var estoqueFinal = contasViewModel.Where(f => f.Descricao.ToUpper().Contains("FINAL")).ToList();
                    contasViewModel = new List<ContaViewModel>();
                    contasViewModel.AddRange(compras);
                    contasViewModel.AddRange(estoqueInicial);
                    contasViewModel.AddRange(estoqueFinal);

                }


                trimestreViewModel.Categorias.Add(new CategoriaViewModel()
                {
                    Descricao = categoria.Descricao,
                    Contas = contasViewModel
                });
            });
            if (trimestreViewModel.Categorias.Where(f => f.Contas.Any(x => x.Codigo == 51203)).FirstOrDefault() != null)
            {
                decimal valor = 0;
                trimestreViewModel.Categorias.Where(f => f.Contas.Any(x => x.Codigo == 51203 || x.Codigo == 51201 || x.Codigo == 51202)).ToList().ForEach(f =>
                {
                    f.Contas.Where(x => x.Codigo == 51203 || x.Codigo == 51201 || x.Codigo == 51202).ToList().ForEach(el =>
                     {
                         valor += el.Lancamentos.Sum(v => v.Valor);
                     });
                });


                //.Sum(x=>x.Contas.Where(f=>))?.Contas.Where(c => c.Codigo == 53).FirstOrDefault();
                var indx = trimestreViewModel.Categorias.FindIndex(f => f.Descricao == "CONTAS A PAGAR");
                trimestreViewModel.Categorias.Where(f => f.Descricao == "CONTAS A PAGAR").ToList().ForEach(el =>
                {

                    var contaF = el.Contas.FindIndex(f => f.Descricao.ToUpper() == "FORNECEDORES");

                    if (valor != 0)
                    {
                        var valores = valor;
                        trimestreViewModel.Categorias[indx].Contas[contaF].Lancamentos[0].ValorStr = Convert.ToString(valores - trimestreViewModel.Categorias[indx].Contas[contaF].Lancamentos.FirstOrDefault().Valor);
                    }
                });

            }



            var categoriaReceita = trimestreViewModel.Categorias.Where(f => f.Descricao.ToUpper() == "Receitas".ToUpper()).FirstOrDefault();


            var recei = categoriaReceita.Contas.Select(f =>
            {

                var lancManual = lancamentos.Where(f => f.ContaId.HasValue).Where(l => (int)f.Id == (int)l.ContaId).FirstOrDefault();
                return new ReceitasViewModel()
                {
                    Codigo = f.Codigo ?? 0,
                    Conta = f.Descricao,
                    Data = competenciasId,
                    Empresa = empresasId,
                    Descricao = lancManual?.Descricao ?? "",
                    Id = lancManual?.Id ?? 0,
                    IdConta = f.Id,
                    TipoLancamento = f.TipoLancamento,
                    ValorCreditoStr = lancManual != null ? lancManual.Valor.ToString() : 0.ToString(),
                    ValorDebitoStr = 0.ToString(),
                    TipoConta = 0
                };
            }).ToList();
            receitas.AddRange(recei);



            var categoriaContas = trimestreViewModel.Categorias.Where(f => f.Descricao.ToUpper() == "Contas a receber".ToUpper()).FirstOrDefault();



            recei = categoriaContas.Contas.Select(f =>
           {

               var lancManual = lancamentos.Where(f => f.ContaId.HasValue).Where(l => (int)f.Id == (int)l.ContaId).FirstOrDefault();
               return new ReceitasViewModel()
               {
                   Codigo = f.Codigo ?? 0,
                   Conta = f.Descricao,
                   Data = competenciasId,
                   Empresa = empresasId,
                   Descricao = lancManual?.Descricao ?? "",
                   Id = lancManual?.Id ?? 0,
                   IdConta = f.Id,
                   TipoLancamento = f.TipoLancamento,
                   ValorCreditoStr = 0.ToString(),
                   ValorDebitoStr = lancManual != null ? lancManual.Valor.ToString() : 0.ToString(),
                   TipoConta = 1
               };
           }).ToList();
            receitas.AddRange(recei);


            receitas = receitas.OrderBy(f => f.TipoConta).ToList();


            var trimestre = CarregarTrimestre(competenciasId, empresasId);
            var estorqueVenda = CarregarVenda(competenciasId, empresasId);

            trimestreViewModel.LancamentosCompra = trimestre.LancamentosCompra;
            trimestreViewModel.LancamentosReceita = trimestre.LancamentosReceita;
            trimestreViewModel.LancamentosDespesa = trimestre.LancamentosDespesa;
            trimestreViewModel.Trimestre = trimestre.Trimestre;
            trimestreViewModel.ProvisoesDepreciacoes = trimestre.ProvisoesDepreciacoes;
            trimestreViewModel.EstoqueVendas = estorqueVenda.EstoqueVendas;
            trimestreViewModel.Receitas = receitas;

            return trimestreViewModel;
        }
        [HttpPost]
        public IActionResult Filtrar(int empresasId, DateTime competenciasId, string url)
        {
            CarregarEmpresasCompetencias(empresasId, competenciasId);

            return RedirectToAction(url.Split("/")[1], url.Split("/")[0]);

            //return View("Index", CarregarCategorias(empresasId, competenciasId));
        }
        [HttpGet]
        public IActionResult Filtrar()
        {
            var date = (DateTime?)null;

            if (ViewBag.CompetenciasSelecionadaId != null)
            {
                date = Convert.ToDateTime(ViewBag.CompetenciasSelecionadaId);
            }
            CarregarEmpresasCompetencias(ViewBag.EmpresaSeleciodaId, date);



            return View("Index", CarregarCategorias((int?)ViewBag.EmpresaSeleciodaId, (DateTime?)date));
        }



        public IActionResult SalvarReceitas(TrimestreViewModel trimestreViewModel)
        {
            AdicionarCompetenciaMesAtual();

            CarregarEmpresasCompetencias();

            if (ViewBag.EmpresaSeleciodaId == null || ViewBag.CompetenciasSelecionadaId == null)
            {
                ViewBag.Message = "Porfavor, selecione uma empresa e uma competencia e filtre!";
                return View("Index", CarregarCategorias());
            }

            var lancamentos = trimestreViewModel.Receitas.Select(x => new Lancamento()
            {
                Id = x.Id,
                ContaId = x.IdConta,
                DataCompetencia = x.Data.HasValue ? x.Data.Value : DateTime.Now,
                Descricao = x.Descricao,
                EmpresaId = x.Empresa.HasValue ? x.Empresa.Value : 0,
                Valor = x.ValorCreditoStr != null ? x.ValorCredito ?? 0 : x.ValorDebito ?? 0
            }).ToList();




            foreach (var item in lancamentos)
            {
                var lanc = _context.Lancamentos.Where(f => f.EmpresaId == item.EmpresaId && f.DataCompetencia == item.DataCompetencia && f.ContaId == item.ContaId).FirstOrDefault();
                if (lanc != null)
                {
                    lanc.ContaId = item.ContaId;
                    lanc.DataCompetencia = item.DataCompetencia;
                    lanc.Descricao = item.Descricao;
                    lanc.EmpresaId = item.EmpresaId;
                    lanc.Valor = item.Valor;
                    _context.Lancamentos.Update(lanc);
                }
                else
                {
                    _context.Lancamentos.Add(item);
                }

                _context.SaveChanges();
            }



            return RedirectToAction("Index");


        }

        [HttpPost]
        public IActionResult Salvar(TrimestreViewModel trimestreViewModel)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            try
            {
                if (ViewBag.EmpresaSeleciodaId == null || ViewBag.CompetenciasSelecionadaId == null)
                {
                    ViewBag.Message = "Porfavor, selecione uma empresa e uma competencia e filtre!";
                    return View("Index", CarregarCategorias());
                }
                DateTime competencia = Convert.ToDateTime(ViewBag.CompetenciasSelecionadaId);
                var empresaId = Convert.ToInt32($"{ViewBag.EmpresaSeleciodaId}");
                //var primeiroLancamento = await_lancamentoDomainService.Salvar(competencia, trimestreViewModel);

                var lancamentoCompetencia = _context.Lancamentos.Where(l => l.DataCompetencia == competencia && l.EmpresaId == empresaId).ToList(); ;
                var estoqueVendas = trimestreViewModel.EstoqueVendas;

                #region ItensVenda
                if (lancamentoCompetencia.Count() == 0)
                {
                    var insertEstoqueVendas = new Venda()
                    {
                        DataCompetencia = competencia,
                        EmpresaId = empresaId,
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
                #endregion
                #region ItensDepreciações
                var provisoesDepreciacoes = trimestreViewModel.ProvisoesDepreciacoes;
                var updateProvisoes = _context.ProvisoesDepreciacoes.Find(provisoesDepreciacoes.Id);
                if (lancamentoCompetencia.Count() == 0 || updateProvisoes == null)
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
                        Apurar = provisoesDepreciacoes.Apurar,
                        CompesacaoPrejuizo = provisoesDepreciacoes.CompesacaoPrejuizo
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
                    updateProvisoes.CompesacaoPrejuizo = provisoesDepreciacoes.CompesacaoPrejuizo;

                    _context.ProvisoesDepreciacoes.Update(updateProvisoes);
                    _context.SaveChanges();
                }
                #endregion

                #region OfxLançamentos
                //trimestreViewModel.Categorias.Where(f => f.Contas.Where(x => x.TipoLancamento == "L").Count() > 0).ToList().ForEach(el =>
                //{
                //    el.Contas.Where(x => x.TipoLancamento == "L").ToList().ForEach(conta =>
                //      {
                //          conta.Lancamentos.Select(x => new OfxLancamento()
                //          {
                //              Id = x.Id,
                //              LancamentoPadraoId = x.Conta,
                //              Data = x.Data,
                //              Descricao = x.Descricao,
                //              ValorOfx = x.Valor,
                //              Valor = x.Valor
                //          });
                //          conta.Lancamentos.ForEach(xxel =>
                //          {

                //          });
                //      });
                //});
                #endregion

                var lancamentosViewModel = trimestreViewModel.Categorias.SelectMany(x => x.Contas.Where(x => string.IsNullOrEmpty(x.TipoLancamento) || x.TipoLancamento == "L").SelectMany(x => x.Lancamentos)).ToList();

                var tste = trimestreViewModel.Categorias.Where(f => f.Descricao.ToUpper().Contains("RESULTADOS")).ToList();


                var lancamentos = lancamentosViewModel.Select(x => new Lancamento()
                {
                    Id = x.Id,
                    ContaId = x.Conta,
                    DataCompetencia = x.Data,
                    Descricao = x.Descricao,
                    EmpresaId = x.Empresa,
                    Valor = x.Valor
                }).OrderBy(x => x.Valor).ToList();

                foreach (var lancamento in lancamentos)
                {
                    var updateLancamento = _context.Lancamentos.Where(f => f.ContaId == lancamento.ContaId && f.DataCompetencia == competencia && f.EmpresaId == empresaId).FirstOrDefault();
                    if (updateLancamento == null && lancamento.Valor == 0)
                    {
                        continue;
                    }

                    if (updateLancamento != null && lancamento.Valor == 0)
                    {
                        _context.Lancamentos.Remove(updateLancamento);
                        _context.SaveChanges();
                        continue;
                    }

                    if (lancamentoCompetencia.Where(f => f.ContaId == lancamento.ContaId).Count() == 0)
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
            catch (Exception e)
            {
                AdicionarCompetenciaMesAtual();
                CarregarEmpresasCompetencias();
                ViewBag.Message = "Erro ao tentar salvar campos manuais";
                return RedirectToAction("Index");
            }
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

            if (!empresaId.HasValue) { return trimestreViewModel; }
            if (!competenciasId.HasValue) { return trimestreViewModel; }

            var contasCorrentes = _context.ContasCorrentes.Where(c => c.EmpresaId == empresaId).ToList();
            var ofxLancamentos = _context.OfxLancamentos.Include(x => x.LancamentoPadrao).ToList()
                .Where(f => contasCorrentes.Any(x => x.Id == f.ContaCorrenteId)
                && f.Data.Year == competenciasId.Value.Year
                && trimestre.Any(x => x == f.Data.Month)).ToList();
            var lancamentos = _context.Lancamentos.Include(x => x.Conta).Where(f => f.DataCompetencia.Year == competenciasId.Value.Year &&
              f.EmpresaId == empresaId.Value && trimestre.Any(x => x == f.DataCompetencia.Month)
            ).ToList();
            List<LancamentoPadrao> contas = _context.LancamentosPadroes.ToList();
            var provisaoDepreciacao = _context.ProvisoesDepreciacoes.Where(f => f.EmpresaId == empresaId && f.DataCompetencia == competenciasId.Value).FirstOrDefault();
            trimestreViewModel.Trimestre = trimestre;



            var categoria = _context.Categorias.Where(f => f.Descricao.ToUpper() == "RECEITAS".ToUpper()).FirstOrDefault();
            var contasReceitas = contas.Where(f => f.CategoriaId == categoria.Id).ToList();


            var categoriasDespesas = _context.Categorias.Where(f => f.Descricao.ToUpper() == "ENCARGOS SOCIAIS".ToUpper()).FirstOrDefault();
            var CategoriaAluguel = _context.Categorias.Where(f => f.Descricao.ToUpper() == "PROVISÕES DE ALUGUÉIS".ToUpper()).FirstOrDefault();
            var CategoriaPis = _context.Categorias.Where(f => f.Descricao.ToUpper() == "PROVISÕES PIS/COFINS/ISS/SIMPLES".ToUpper()).FirstOrDefault();
            var contasDespesas = contas.Where(f => f.Descricao.ToUpper().Contains("DESP".ToUpper()) || f.Codigo == 9 || f.Codigo == 10 || f.Codigo == 11).ToList();

            contasDespesas.AddRange(contas.Where(f => f.CategoriaId == categoriasDespesas.Id).ToList());
            contasDespesas.AddRange(contas.Where(f => f.CategoriaId == CategoriaAluguel.Id).ToList());
            contasDespesas.AddRange(contas.Where(f => f.CategoriaId == CategoriaPis.Id).ToList());

            contasDespesas = contasDespesas.Where(f => f.Codigo != 156 && f.Codigo != 181 && f.Codigo != 182 && f.Codigo != 183 && f.Codigo != 184 && f.Codigo != 185 && f.Codigo != 13601 && f.Codigo != 13644 && f.Codigo != 21904).ToList();


            contasDespesas = contasDespesas.Where(f => !(f.Descricao.ToUpper() == "IRRF Aluguel".ToUpper())).ToList();
            //contasDespesas = contasDespesas.Where(f => !(f.Descricao.ToUpper() == "COFINS (5856/2172)".ToUpper())).ToList();
            //contasDespesas = contasDespesas.Where(f => !(f.Descricao.ToUpper() == "Pis (6912/8109)".ToUpper())).ToList();
            //contasDespesas = contasDespesas.Where(f => !(f.Descricao.ToUpper() == "ISS Outros".ToUpper())).ToList();


            contasDespesas = contasDespesas.Distinct().ToList();
            var teste = string.Join(",", contasDespesas.Select(f => $"{f.Codigo}-{f.Descricao}").ToList());





            trimestre.ToList().ForEach(el =>
            {
                ///  Estoque Inicial
                lancamentos.Where(f => (f.ContaId == 100 || f.ContaId == 171 || f.ContaId == 170) && f.DataCompetencia.Month == el).ToList().ForEach(lancamento =>
                 {
                     trimestreViewModel.LancamentosCompra.Add(new LancamentoViewModel()
                     {
                         Id = lancamento.Id,
                         Data = lancamento.DataCompetencia,
                         Empresa = lancamento.EmpresaId,
                         Conta = 100,
                         Descricao = lancamento.Descricao,
                         ValorStr = lancamento.Valor.ToString()
                     });
                 });
                ///  Compras
                lancamentos.Where(f => (f.ContaId == 99 || f.ContaId == 174 || f.ContaId == 175) && f.DataCompetencia.Month == el).ToList().ForEach(lancamento =>
                {
                    trimestreViewModel.LancamentosCompra.Add(new LancamentoViewModel()
                    {
                        Id = lancamento.Id,
                        Data = lancamento.DataCompetencia,
                        Empresa = lancamento.EmpresaId,
                        Conta = null,
                        Descricao = lancamento.Descricao,
                        ValorStr = lancamento.Valor.ToString()
                    });
                });


                ///  Estoque Final
                lancamentos.Where(f => (f.ContaId == 101 || f.ContaId == 172 || f.ContaId == 173) && f.DataCompetencia.Month == el).ToList().ForEach(lancamento =>
                {
                    trimestreViewModel.LancamentosCompra.Add(new LancamentoViewModel()
                    {
                        Id = lancamento.Id,
                        Data = lancamento.DataCompetencia,
                        Empresa = lancamento.EmpresaId,
                        Conta = 101,
                        Descricao = lancamento.Descricao,
                        ValorStr = lancamento.Valor.ToString()
                    });
                });
                // Receitas
                contasReceitas.ForEach(xel =>
                {
                    ofxLancamentos.Where(f => f.LancamentoPadraoId == xel.Id && f.Data.Month == el).ToList().ForEach(lancamento =>
                    {
                        trimestreViewModel.LancamentosReceita.Add(new LancamentoViewModel()
                        {
                            Id = lancamento.Id,
                            Data = lancamento.Data,
                            Empresa = empresaId.Value,
                            Conta = xel.Id,
                            Descricao = lancamento.Descricao,
                            ValorStr = lancamento.ValorOfx.ToString()
                        });
                    });

                    lancamentos.Where(f => f.ContaId == xel.Id && f.DataCompetencia.Month == el).ToList().ForEach(lancamento =>
                    {


                        trimestreViewModel.LancamentosReceita.Add(new LancamentoViewModel()
                        {
                            Id = lancamento.Id,
                            Data = lancamento.DataCompetencia,
                            Empresa = lancamento.EmpresaId,
                            Conta = xel.Id,
                            Descricao = xel.Descricao,
                            ValorStr = lancamento.Valor.ToString()
                        });
                    });


                });


                // Despesa
                contasDespesas.ForEach(xel =>
                {
                    lancamentos.Where(f => f.ContaId == xel.Id && f.DataCompetencia.Month == el).ToList().ForEach(lancamento =>
                    {


                        trimestreViewModel.LancamentosDespesa.Add(new LancamentoViewModel()
                        {
                            Id = lancamento.Id,
                            Data = lancamento.DataCompetencia,
                            Empresa = lancamento.EmpresaId,
                            Conta = xel.Id,
                            Descricao = xel.Descricao,
                            ValorStr = lancamento.Valor.ToString()
                        });
                    });

                    ofxLancamentos.Where(f => f.LancamentoPadraoId == xel.Id && f.Data.Month == el).ToList().ForEach(lancamento =>
                      {
                          trimestreViewModel.LancamentosDespesa.Add(new LancamentoViewModel()
                          {
                              Id = lancamento.Id,
                              Data = lancamento.Data,
                              Empresa = empresaId.Value,
                              Conta = xel.Id,
                              Descricao = xel.Descricao,
                              ValorStr = lancamento.ValorOfx.ToString()
                          });
                      });
                });

                var dd = new LancamentoViewModel();
                dd.ValorStr = (
(lancamentos.Where(f => f.DataCompetencia.Month == el).Where(f => f.Conta.Descricao.ToUpper() == "Salário Bruto".ToUpper()
|| f.Conta.Descricao.ToUpper() == "INSS(TOTAL FOLHA)".ToUpper()
|| f.Conta.Descricao.ToUpper() == "FGTS".ToUpper()).Sum(x => x.Valor)

                - lancamentos.Where(f => f.DataCompetencia.Month == el).Where(f => f.Conta.Descricao.ToUpper() == "INSS - SEGURADOS".ToUpper()
                 || f.Conta.Descricao.ToUpper() == "FÉRIAS".ToUpper()
                 || f.Conta.Descricao.ToUpper() == "DESCONTOS / ATRASOS".ToUpper()).Sum(x => x.Valor)
                 ) * -1
                ).ToString();
                trimestreViewModel.LancamentosDespesa.Add(new LancamentoViewModel()
                {
                    Id = 777,
                    Data = new DateTime(competenciasId.Value.Year, el, 1),
                    Empresa = empresaId.Value,
                    Conta = null,
                    Descricao = "Total despesas Folha".ToUpper(),
                    ValorStr = dd.ValorStr
                });

            });


            foreach (var competencia in trimestre)
            {
                foreach (var contaCorrente in contasCorrentes)
                {
                    //foreach (var lancamento in OfxLancamentos.Where(l => l.Data.Month == competencia
                    //                                        && l.LancamentoPadrao?.TipoContaId == (int)ETipoConta.Compras
                    //                                        && l.ContaCorrenteId == contaCorrente.Id))
                    //{

                    //}

                    //foreach (var lancamento in OfxLancamentos.Where(l => l.Data.Month == competencia
                    //                                        && l.LancamentoPadrao?.TipoContaId == (int)ETipoConta.EstoqueInicial
                    //                                        && l.ContaCorrenteId == contaCorrente.Id))
                    //{
                    //    trimestreViewModel.LancamentosCompra.Add(new LancamentoViewModel()
                    //    {
                    //        Id = lancamento.Id,
                    //        Data = lancamento.Data,
                    //        Empresa = contaCorrente.EmpresaId,
                    //        Conta = lancamento.LancamentoPadraoId,
                    //        Descricao = lancamento.Descricao,
                    //        ValorStr = lancamento.ValorOfx.ToString()
                    //    });
                    //}

                    //foreach (var lancamento in OfxLancamentos.Where(l => l.Data.Month == competencia
                    //                                        && l.LancamentoPadrao?.TipoContaId == (int)ETipoConta.EstoqueFinal
                    //                                        && l.ContaCorrenteId == contaCorrente.Id))
                    //{
                    //    trimestreViewModel.LancamentosCompra.Add(new LancamentoViewModel()
                    //    {
                    //        Id = lancamento.Id,
                    //        Data = lancamento.Data,
                    //        Empresa = contaCorrente.EmpresaId,
                    //        Conta = lancamento.LancamentoPadraoId,
                    //        Descricao = lancamento.Descricao,
                    //        ValorStr = lancamento.ValorOfx.ToString()
                    //    });
                    //}

                    //foreach (var lancamento in OfxLancamentos.Where(l => l.Data.Month == competencia
                    //                                        && l.LancamentoPadrao?.TipoContaId == (int)ETipoConta.Receitas
                    //                                        && l.ContaCorrenteId == contaCorrente.Id))
                    //{
                    //    trimestreViewModel.LancamentosReceita.Add(new LancamentoViewModel()
                    //    {
                    //        Id = lancamento.Id,
                    //        Data = lancamento.Data,
                    //        Empresa = contaCorrente.EmpresaId,
                    //        Conta = lancamento.LancamentoPadraoId,
                    //        Descricao = lancamento.Descricao,
                    //        ValorStr = lancamento.ValorOfx.ToString()
                    //    });
                    //}

                    //foreach (var conta in contas.Where(c => c.TipoContaId == (int)ETipoConta.Despesas))
                    //{
                    //    if (conta.Lancamentos == null)
                    //    {
                    //        continue;
                    //    }

                    //    foreach (var lancamento in lancamentos.Where(l => l.EmpresaId == empresaId
                    //                                                   && l.DataCompetencia.Month == competencia))
                    //    {
                    //        trimestreViewModel.LancamentosDespesa.Add(new LancamentoViewModel()
                    //        {
                    //            Id = lancamento.Id,
                    //            Data = lancamento.DataCompetencia,
                    //            Empresa = lancamento.EmpresaId,
                    //            Conta = lancamento.ContaId,
                    //            Descricao = lancamento.Descricao,
                    //            ValorStr = lancamento.Valor.ToString()
                    //        });
                    //    }
                    //    foreach (var lancamento in OfxLancamentos.Where(l => l.Data.Month == competencia))
                    //    {
                    //        trimestreViewModel.LancamentosDespesa.Add(new LancamentoViewModel()
                    //        {
                    //            Id = lancamento.Id,
                    //            Data = lancamento.Data,
                    //            Empresa = empresaId.Value,
                    //            Conta = lancamento.LancamentoPadraoId,
                    //            Descricao = lancamento.Descricao,
                    //            ValorStr = lancamento.ValorOfx.ToString()
                    //        });
                    //    }
                    //}
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
            //var contas = _context.LancamentosPadroes.ToList();
            var lancamentos = _context.Lancamentos
                .Include(f => f.Empresa)
                .Include(f => f.Conta).ThenInclude(f => f.Categoria)
                .Where(l => l.DataCompetencia == competenciasId && l.EmpresaId == empresaId && l.Valor > 0)
                .OrderBy(f => f.DataCompetencia)
                .ToList();

            lancamentos = lancamentos.Where(f => (f.Conta.Descricao.ToUpper().Trim() == "( + ) COMPRAS DE MERCADORIAS"
            || f.Conta.Descricao.ToUpper().Trim() == "( = ) ESTOQUE INICIAL MERCADORIAS"
            || f.Conta.Descricao.ToUpper().Trim() == "( - ) ESTOQUE FINAL MERCADORIAS"
            || f.Conta.Descricao.ToUpper().Trim() == "VENDAS DE MERCADORIAS - COMBUSTÍVEIS"
            || f.Conta.Codigo == 119
            || f.Conta.Codigo == 51403
            || f.Conta.Codigo == 51103
            || f.Conta.Codigo == 51203
            || f.Conta.Codigo == 98
            || f.Conta.Codigo == 99
            || f.Conta.Codigo == 155
            || f.Conta.Categoria.Descricao.ToUpper() == "CONTAS A RECEBER"
            )).ToList();

            var ofxLancamentos = _context.OfxLancamentos
                .Include(f => f.ContaCorrente)
                .ThenInclude(x => x.Empresa)
                .Include(f => f.ContaCorrente)
                .ThenInclude(f => f.BancoOfx)
                .Include(f => f.LancamentoPadrao)
                .Include(f => f.Lote)
                .Where(l => l.Lote.CompetenciaId == competenciasId && l.Lote.EmpresaId == empresaId && (!l.Inativar.HasValue || !l.Inativar.Value))
                .OrderBy(f => f.Data)
                .ThenBy(f => f.Documento)
                .ToList();

            Empresa empresa = null;

            if (lancamentos.Count > 0)
            {
                empresa = lancamentos.FirstOrDefault().Empresa;
            }
            else
            if (ofxLancamentos.Count > 0)
            {
                empresa = ofxLancamentos.FirstOrDefault().ContaCorrente.Empresa;
            }
            else
            {
                empresa = _context.Empresas.FirstOrDefault(f => f.Codigo == empresaId);
            }

            var lancamentosContabeis = new List<TextoViewModel>();

            StringBuilder builder = new StringBuilder();

            //builder.AppendLine($"|0000|{empresa.Cnpj.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty)}|");
            CultureInfo pt = new CultureInfo("pt-BR");
            string documentoAux = string.Empty;
            ofxLancamentos.ForEach(f =>
            {

                var contaCredito = f.LancamentoPadrao.ContaCreditoId.ToString().Replace("11201", f.ContaCorrente.BancoOfx.CodigoContabil);  // f.ValorOfx < 0 ? f.LancamentoPadrao.ContaDebitoId : f.LancamentoPadrao.ContaCreditoId;
                var contaDebito = f.LancamentoPadrao.ContaDebitoId.ToString().Replace("11201", f.ContaCorrente.BancoOfx.CodigoContabil);//f.ValorOfx < 0 ? f.LancamentoPadrao.ContaCreditoId : f.LancamentoPadrao.ContaDebitoId;
                if (documentoAux.Equals(f.Documento))
                {
                    builder.AppendLine($"|6100|{f.Data.ToString("dd/MM/yyyy")}|{contaDebito}|{contaCredito}|{Math.Abs(f.ValorOfx).ToString(pt)}|{f.LancamentoPadrao.LancamentoHistorico}|{f.Descricao}||||");
                }
                else
                {
                    var qtd = ofxLancamentos.Where(x => x.Documento == f.Documento).Count();
                    var tipo = qtd > 1 ? "V" : "X";

                    builder.AppendLine($"|6000|{tipo}||||");
                    builder.AppendLine($"|6100|{f.Data.ToString("dd/MM/yyyy")}|{contaDebito}|{contaCredito}|{Math.Abs(f.ValorOfx).ToString(pt)}|{f.LancamentoPadrao.LancamentoHistorico}|{f.Descricao}||||");
                }


                documentoAux = f.Documento;
            });
            lancamentos.ForEach(el =>
            {
                var tipo = "X";
                builder.AppendLine($"|6000|{tipo}||||");
                builder.AppendLine($"|6100|{el.DataCompetencia.ToString("dd/MM/yyyy")}|{el.Conta.ContaDebitoId}|{el.Conta.ContaCreditoId}|{el.Valor.ToString(pt)}||{el.Conta.Descricao}||||");
            });

            //foreach (var conta in contas)
            //{
            //    foreach (var lancamento in lancamentos.Where(l => l.ContaId == conta.Id))
            //    {
            //        lancamentosContabeis.Add(new TextoViewModel()
            //        {
            //            Data = competenciasId,
            //            CodigoContaDebito = conta.ContaDebitoId,
            //            CodigoContaCredito = conta.ContaCreditoId,
            //            Valor = lancamento.Valor,
            //            CodigoHistorico = 10,
            //            ComplementoHistorico = string.Empty,
            //            IniciaLote = 1,
            //            CodigoMatrizFilial = empresaId,
            //            CentroCustoDebito = 1,
            //            CentroCustoCredito = 1,
            //        });
            //    }
            //}

            //var conteudoArquivo = string.Empty;

            //foreach (var lancamento in lancamentosContabeis)
            //{
            //    conteudoArquivo += $"{lancamento.Data.Value.ToShortDateString()};{lancamento.CodigoContaDebito};{lancamento.CodigoContaCredito};" +
            //        $"{lancamento.Valor};{lancamento.CodigoHistorico};{lancamento.ComplementoHistorico};" +
            //        $"{lancamento.IniciaLote};{lancamento.CodigoMatrizFilial};{lancamento.CentroCustoDebito};" +
            //        $"{lancamento.CentroCustoCredito};{Environment.NewLine}";
            //}

            //var conteudoArquivo = _lancamentoDomainService.GerarArquivo(empresaId, competenciasId);

            var stream = new MemoryStream(Encoding.ASCII.GetBytes(builder.ToString()));
            return new FileStreamResult(stream, "text/plain")
            {
                FileDownloadName = $"Movimento_Financeiro_{empresa.RazaoSocial}_{competenciasId.ToString("MM-yyyy")}_Dominio.txt"
            };
        }
    }
}