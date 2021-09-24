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
        [HttpPost]
        public IActionResult Index()
        {
            List<Conta> contas = context.Contas.ToList();
            List<Categoria> categorias = context.Categorias.ToList();
            List<Empresa> empresas = context.Empresas.ToList();
            List<Competencia> competencias = context.Competencias.ToList();
            List<Lancamento> lancamentos = context.Lancamentos.ToList();

            //melhorar essa viewbag
            ViewBag.Contas = contas;
            ViewBag.Categorias = categorias;
            ViewBag.LancamentosId = lancamentos;
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

            ViewBag.Contas = contas;
            ViewBag.Categorias = categorias;
            ViewBag.CompetenciasId = new SelectList(
                competencias.Select(c => new { Value = c.Data.ToShortDateString(), Text = c.Data.ToString("MM/yyyy") })
                , "Value", "Text");
            ViewBag.EmpresaId = new SelectList(empresas, "Codigo", "RazaoSocial");

            if (competencias.Any(c => c.Data == CompetenciasId))
            {
                foreach (var lancamento in lancamentos.Where(l => l.EmpresaId == EmpresaId && l.DataCompetencia == CompetenciasId))
                {
                    ViewBag.LancamentosId = lancamento;
                }
            }
            else
            {
                Competencia competencia = new Competencia();
                competencia.Data = CompetenciasId;
                context.Add(competencia);
                context.SaveChanges();
            }
            return View("Index");
        }

    }
}
