using Demonstrativo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace Demonstrativo.Controllers
{
    public class LancamentoController : Controller
    {
        Context context = new();
        public IActionResult Index()
        {
            AdicionarCompetenciaMesAtual();

            CarregarEmpresasCompetencias();

            return View(CarregarCategorias());
        }

        private void AdicionarCompetenciaMesAtual()
        {
            DateTime competenciaAtual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);

            if(context.Competencias.Any(c => c.Data == competenciaAtual))
            {
                return;
            }

            Competencia competencia = new Competencia()
            {
                Data = competenciaAtual
            };

            context.Competencias.Add(competencia);
            context.SaveChanges();
        }

        private void CarregarEmpresasCompetencias(int? empresaId = null, DateTime? competenciasId = null)
        {
            List<Empresa> empresas = context.Empresas.ToList();
            List<Competencia> competencias = context.Competencias.ToList();

            ViewBag.CompetenciasId = new SelectList(
               competencias.Select(c => new { Value = c.Data.ToShortDateString(), Text = c.Data.ToString("MM/yyyy") })
               , "Value", "Text", competenciasId.HasValue ? competenciasId.Value.ToShortDateString() : competenciasId);

            ViewBag.EmpresaId = new SelectList(empresas, "Codigo", "RazaoSocial", empresaId);
        }

        private TrimestreViewModel CarregarCategorias(int? empresaId=null, DateTime? competenciasId=null)
        {
            var trimestreViewModel = new TrimestreViewModel();

            List<Conta> contas = context.Contas.ToList();
            List<Categoria> categorias = context.Categorias.ToList();

            var lancamentos = new List<Lancamento>();

            if (empresaId.HasValue && competenciasId.HasValue)
            {
                lancamentos =  context.Lancamentos.Where(x => x.EmpresaId == empresaId && x.DataCompetencia == competenciasId)
                    .ToList();
            }

            foreach (var categoria in categorias)
            {
                var contasViewModel = new List<ContaViewModel>();

                if(categoria.Id == 19 || categoria.Id == 25)
                {
                    contas.Add(new Conta() { CategoriaId = categoria.Id});
                    contas.Add(new Conta() { CategoriaId = categoria.Id });
                    contas.Add(new Conta() { CategoriaId = categoria.Id });
                    contas.Add(new Conta() { CategoriaId = categoria.Id });
                    contas.Add(new Conta() { CategoriaId = categoria.Id });
                }

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
                            Valor = lancamento.Valor
                        });

                    }

                    if(conta.Codigo == 38 || conta.Codigo == 36)
                    {
                        lancamentosViewModel.Add(new LancamentoViewModel() { PodeDigitarDescricao = true});
                        lancamentosViewModel.Add(new LancamentoViewModel() { PodeDigitarDescricao = true });
                        lancamentosViewModel.Add(new LancamentoViewModel() { PodeDigitarDescricao = true });
                        lancamentosViewModel.Add(new LancamentoViewModel() { PodeDigitarDescricao = true });
                        lancamentosViewModel.Add(new LancamentoViewModel() { PodeDigitarDescricao = true });
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

            trimestreViewModel.LancamentosCompra = trimestre.LancamentosCompra;
            trimestreViewModel.LancamentosReceita = trimestre.LancamentosReceita;
            trimestreViewModel.LancamentosDespesa = trimestre.LancamentosDespesa;
            trimestreViewModel.Trimestre = trimestre.Trimestre;
            trimestreViewModel.ProvisoesDepreciacoes = trimestre.ProvisoesDepreciacoes;

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
        public IActionResult Salvar(List<Lancamento> lancamentos, 
            int provisaoId,
            DateTime provisaoData,
            int provisaoEmpresa,
            decimal? decimo,
            decimal? ferias,
            decimal? depreciacao,
            decimal? prejuizo,
            bool calcularCompensacao)
        {
            if (provisaoId == 0)
            {
                var insertProvisoes = new ProvisoesDepreciacao();
                insertProvisoes.DataCompetencia = provisaoData;
                insertProvisoes.EmpresaId = provisaoEmpresa;
                insertProvisoes.DecimoTerceiro = decimo;
                insertProvisoes.Ferias = ferias;
                insertProvisoes.Depreciacao = depreciacao;
                insertProvisoes.SaldoPrejuizo = prejuizo;
                insertProvisoes.CalcularCompensacao = calcularCompensacao;

                context.ProvisoesDepreciacoes.Add(insertProvisoes);
                context.SaveChanges();
            }
            else
            {
                var updateProvisoes = context.ProvisoesDepreciacoes.Find(Convert.ToInt32(provisaoId));

                updateProvisoes.DataCompetencia = provisaoData;
                updateProvisoes.EmpresaId = provisaoEmpresa;
                updateProvisoes.DecimoTerceiro = decimo;
                updateProvisoes.Ferias = ferias;
                updateProvisoes.Depreciacao = depreciacao;
                updateProvisoes.SaldoPrejuizo = prejuizo;
                updateProvisoes.CalcularCompensacao = calcularCompensacao;

                context.ProvisoesDepreciacoes.Update(updateProvisoes);
                context.SaveChanges();
            }

            foreach (var lancamento in lancamentos)
            {
                if (lancamento.Id == 0 && lancamento.Valor == 0)
                {
                    continue;
                }

                if(lancamento.Id != 0 && lancamento.Valor == 0)
                {
                    
                    context.Lancamentos.Remove(lancamento);
                    context.SaveChanges();
                    continue;
                }

                if (lancamento.Id == 0)
                {
                    var insertLancamento = new Lancamento();
                    insertLancamento.ContaId = lancamento.ContaId;
                    insertLancamento.EmpresaId = lancamento.EmpresaId;
                    insertLancamento.DataCompetencia = lancamento.DataCompetencia;
                    insertLancamento.Descricao = lancamento.Descricao;
                    insertLancamento.Valor = lancamento.Valor;

                    context.Lancamentos.Add(insertLancamento);
                    context.SaveChanges();
                }
                else
                {
                    var updateLancamento = context.Lancamentos.Find(Convert.ToInt32(lancamento.Id));

                    updateLancamento.Descricao = lancamento.Descricao;
                    updateLancamento.Valor = lancamento.Valor;

                    context.Lancamentos.Update(updateLancamento);
                    context.SaveChanges();
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
                return SomarTrimestre(trimestre, empresaId);
            }
            else if (mes >= 4 && mes < 7)
            {
                int[] trimestre = { 4, 5, 6 };
                return SomarTrimestre(trimestre, empresaId);
            }
            else if (mes >= 7 && mes < 10)
            {
                int[] trimestre = { 7, 8, 9 };
                return SomarTrimestre(trimestre, empresaId);
            }
            else
            {
                int[] trimestre = { 10, 11, 12 };
                return SomarTrimestre(trimestre, empresaId);
            }

        }
        
        //SOMA Trimestre
        public TrimestreViewModel SomarTrimestre(int[]? trimestre, int? empresaId)
        {
            var trimestreViewModel = new TrimestreViewModel();

            List<Lancamento> lancamentos = context.Lancamentos.ToList();
            List<Conta> contas = context.Contas.ToList();
            List<ProvisoesDepreciacao> provisoes = context.ProvisoesDepreciacoes.ToList();

            //ADD TRIMESTRE
            trimestreViewModel.Trimestre = trimestre;

            foreach (var competencia in trimestre)
            {
                //TRIMESTRE COMPRAS
                foreach (var lancamento in lancamentos.Where(l => l.EmpresaId == empresaId && l.DataCompetencia.Month == competencia && l.ContaId == 99))
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
                //TRIMESTRE SALDO ESTOQUE INICIAL
                foreach (var lancamento in lancamentos.Where(l => l.EmpresaId == empresaId && l.DataCompetencia.Month == competencia && l.ContaId == 100))
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
                //TRIMESTRE SALDO ESTOQUE FINAL
                foreach (var lancamento in lancamentos.Where(l => l.EmpresaId == empresaId && l.DataCompetencia.Month == competencia && l.ContaId == 101))
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
                //TRIMESTRE RECEITAS
                foreach (var lancamento in lancamentos.Where(l => l.EmpresaId == empresaId && l.DataCompetencia.Month == competencia && l.ContaId >= 102 && l.ContaId <= 111))
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
                //TRIMESTRE DESPESAS 
                foreach (var conta in contas.Where(c => (c.CategoriaId > 1 && c.CategoriaId < 7 || 
                            c.CategoriaId == 10 || 
                            c.CategoriaId == 12 || 
                            c.CategoriaId == 13 || 
                            c.CategoriaId == 15 || 
                            c.CategoriaId == 20 || 
                            c.CategoriaId == 21 || 
                            c.CategoriaId == 23 || 
                            c.CategoriaId == 24) && c.Id != 11 && c.Id != 12 && c.Id != 27 && c.Id != 33 && c.Id != 130))
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
                //TRIMESTRE PROVISOES E DEPRECIASOES
                foreach (var provisoesDepreciacao in provisoes.Where(p => p.EmpresaId == empresaId && p.DataCompetencia.Month == competencia))
                {
                    trimestreViewModel.ProvisoesDepreciacoes.Add(new ProvisoesDepreciacoesViewModel()
                    {
                        Id = provisoesDepreciacao.Id,
                        Data = provisoesDepreciacao.DataCompetencia,
                        Empresa = provisoesDepreciacao.EmpresaId,
                        Ferias = provisoesDepreciacao.Ferias,
                        DecimoTerceiro = provisoesDepreciacao.DecimoTerceiro,
                        Depreciacao = provisoesDepreciacao.Depreciacao,
                        SaldoPrejuizo = provisoesDepreciacao.SaldoPrejuizo,
                        CalcularCompesacao = provisoesDepreciacao.CalcularCompensacao
                    }) ;
                }
            }

            return trimestreViewModel;
        }
    }
}