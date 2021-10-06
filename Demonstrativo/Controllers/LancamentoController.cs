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
            List<Conta> contas = context.Contas.ToList();
            List<Categoria> categorias = context.Categorias.ToList();
            List<Empresa> empresas = context.Empresas.ToList();
            List<Competencia> competencias = context.Competencias.ToList();
            List<Lancamento> lancamentos = context.Lancamentos.ToList();

            var categoriasViewModel = new List<CategoriaViewModel>();


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
                            Valor = lancamento.Valor,
                            Descricao = lancamento.Descricao
                        });

                        ViewBag.ContasViewModel = contasViewModel;
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



            ViewBag.ContasViewModel = contasViewModel;
            ViewBag.CategoriasViewModel = categoriasViewModel;

            ViewBag.CompetenciasId = new SelectList(
                competencias.Select(c => new { Value = c.Data.ToShortDateString(), Text = c.Data.ToString("MM/yyyy") })
                , "Value", "Text");
            ViewBag.EmpresaId = new SelectList(empresas, "Codigo", "RazaoSocial");

            return View();
        }

        [HttpPost]
        public ActionResult Filtrar(int EmpresaId, DateTime CompetenciasId)
        {
            List<Empresa> empresas = context.Empresas.ToList();
            List<Competencia> competencias = context.Competencias.ToList();
            List<Lancamento> lancamentos = context.Lancamentos.ToList();
            List<Conta> contas = context.Contas.ToList();
            List<Categoria> categorias = context.Categorias.ToList();

            ViewBag.Competencias = competencias;
            ViewBag.Contas = contas;
            ViewBag.Categorias = categorias;
            ViewBag.CompetenciasId = new SelectList(
                competencias.Select(c => new { Value = c.Data.ToShortDateString(), Text = c.Data.ToString("MM/yyyy") })
                , "Value", "Text", CompetenciasId.ToShortDateString());
            ViewBag.EmpresaId = new SelectList(empresas, "Codigo", "RazaoSocial",EmpresaId);
            ViewBag.EmpresaSeleciodaId = EmpresaId;
            ViewBag.CompetenciasSelecionadaId = CompetenciasId;

            if (competencias.Any(c => c.Data == CompetenciasId))
            {
                ViewBag.LancamentosId = lancamentos.Where(l => l.EmpresaId == EmpresaId && l.DataCompetencia == CompetenciasId).ToList();             
            }
            else
            {
                Competencia competencia = new Competencia();
                competencia.Data = CompetenciasId;
                context.Competencias.Add(competencia);
                context.SaveChanges();
            }

            return View("Index");
        }

        /*[HttpPost]
        public ActionResult Inserir(List<CategoriaViewModel> categoriaViewModels)
        {
            for (int i = 0; i < codigo.Length; i++)
            {
                if (string.IsNullOrEmpty(name[i]))
                {
                    continue;
                }

                if (string.IsNullOrEmpty(lancamentoId[i]))
                {
                    var lancamento = new Lancamento();
                    lancamento.ContaId = Convert.ToInt32(codigo[i]);
                    lancamento.EmpresaId = Convert.ToInt32(empresa);
                    lancamento.DataCompetencia = Convert.ToDateTime(competencia);
                    lancamento.Descricao = descricao[i];
                    lancamento.Valor = Convert.ToDecimal(name[i]);

                    context.Lancamentos.Add(lancamento);
                    context.SaveChanges();
                }
                else
                {
                    var lancamento = context.Lancamentos.Find(Convert.ToInt32(lancamentoId[i]));

                    lancamento.ContaId = Convert.ToInt32(codigo[i]);
                    lancamento.EmpresaId = Convert.ToInt32(empresa);
                    lancamento.DataCompetencia = Convert.ToDateTime(competencia);
                    lancamento.Descricao = descricao[i];
                    lancamento.Valor = Convert.ToDecimal(name[i]);

                    context.Lancamentos.Update(lancamento);
                    context.SaveChanges();

                }                
            }

            return Filtrar(Convert.ToInt32(empresa), Convert.ToDateTime(competencia));
        }*/

    }
}
