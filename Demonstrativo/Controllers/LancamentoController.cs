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

        private List<TrimestreViewModel> CarregarCategorias(int? empresaId=null, DateTime? competenciasId=null)
        {
            var trimestreViewModel = new List<TrimestreViewModel>();

            List<Conta> contas = context.Contas.ToList();
            List<Categoria> categorias = context.Categorias.ToList();

            var lancamentos = new List<Lancamento>();

            if (empresaId.HasValue && competenciasId.HasValue)
            {
                lancamentos =  context.Lancamentos.Where(x => x.EmpresaId == empresaId && x.DataCompetencia == competenciasId)
                    .ToList();
            }

            var categoriasViewModel = new List<CategoriaViewModel>();

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

                categoriasViewModel.Add(new CategoriaViewModel()
                {
                    Descricao = categoria.Descricao,
                    Contas = contasViewModel
                });

                trimestreViewModel.Add(new TrimestreViewModel()
                {
                    Categorias = categoriasViewModel
                });
            }

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
        public IActionResult Salvar(List<Lancamento> lancamentos)
        {
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
                return Filtrar(primeiroLancamento.EmpresaId,primeiroLancamento.DataCompetencia );
            else
                return RedirectToAction("Index");
        }
    
        public void CarregarTrimestre(DateTime? competenciasId = null)
        {
            if (competenciasId == null)
            {
                return;
            }
            else
            {
                var mes = competenciasId.Value.Month;
                if (mes < 4)
                {
                    int[] trimestre = {1,2,3 };
                    ViewBag.CompetenciasTrimestre = trimestre.ToList();
                    SomarCompras();
                }
                else if (mes >= 4 && mes < 7)
                {
                    int[] trimestre = { 4, 5, 6 };
                    ViewBag.CompetenciasTrimestre = trimestre.ToList();
                    SomarCompras();
                }
                else if (mes >= 7 && mes < 10)
                {
                    int[] trimestre = { 7, 8, 9 };
                    ViewBag.CompetenciasTrimestre = trimestre.ToList();
                    SomarCompras();
                }
                else if (mes >= 10)
                {
                    int[] trimestre = { 10, 11, 12 };
                    ViewBag.CompetenciasTrimestre = trimestre.ToList();
                    SomarCompras();
                }
            }
        }
        public List<TrimestreViewModel> SomarCompras()
        {
            var empresaId = ViewBag.EmpresaSeleciodaId;
            var competenciasId = ViewBag.CompetenciaSelecionadaId;
            var lancamentosTrimestre = new List<LancamentoViewModel>();
            var trimestreViewModel = new List<TrimestreViewModel>();

            List<Lancamento> lancamentos = context.Lancamentos.ToList();
            foreach (var competencia in ViewBag.CompetenciasTrimestre)
            {
                foreach (var lancamento in lancamentos.Where(l => l.EmpresaId == empresaId && l.DataCompetencia.Month == competencia && l.ContaId == 99))
                {
                    lancamentosTrimestre.Add(new LancamentoViewModel() {
                        Id = lancamento.Id,
                        Data = lancamento.DataCompetencia,
                        Empresa = lancamento.EmpresaId,
                        Conta = lancamento.ContaId,
                        Descricao = lancamento.Descricao,
                        Valor = lancamento.Valor
                    } );
                }
                trimestreViewModel.Add(new TrimestreViewModel()
                {
                    LancamentosCompra = lancamentosTrimestre.ToList()
                }) ;
            }

            return trimestreViewModel;
        }
    }
}