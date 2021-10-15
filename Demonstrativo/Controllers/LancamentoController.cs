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
        Context context = new Context();
        public IActionResult Index()
        {
            AdicionarCompetenciaMesAtual();

            CarregarEmpresasCompetencias();

            CarregarCategorias();

            return View();
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

        private void CarregarEmpresasCompetencias()
        {
            List<Empresa> empresas = context.Empresas.ToList();
            List<Competencia> competencias = context.Competencias.ToList();

            ViewBag.CompetenciasId = new SelectList(
               competencias.Select(c => new { Value = c.Data.ToShortDateString(), Text = c.Data.ToString("MM/yyyy") })
               , "Value", "Text");

            ViewBag.EmpresaId = new SelectList(empresas, "Codigo", "RazaoSocial");
        }

        private void CarregarCategorias(int? empresaId=null, DateTime? competenciasId=null)
        {
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
            }
            
            ViewBag.CategoriasViewModel = categoriasViewModel;
        }

        [HttpPost]
        public IActionResult Filtrar(int empresaId, DateTime competenciasId)
        {
            CarregarCategorias(empresaId,competenciasId);

            CarregarEmpresasCompetencias();

            ViewBag.EmpresaSeleciodaId = empresaId;
            ViewBag.CompetenciasSelecionadaId = competenciasId.ToString("yyyy-MM-dd");

            return View("Index");
        }

        [HttpPost]
        public IActionResult Salvar(List<Lancamento> lancamentos)
        {
            foreach (var lancamento in lancamentos)
            {
                if (lancamento.Valor == 0)
                {
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
              
            return Filtrar(primeiroLancamento.EmpresaId, primeiroLancamento.DataCompetencia);
        }
    }
}
