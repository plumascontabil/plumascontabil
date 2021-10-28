using Demonstrativo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Controllers
{
    public class ResultadoController : Controller
    {

        Context context = new();
        public IActionResult Index()
        {
            CarregarEmpresasCompetencias();
            return View();
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

        public void Compras(int EmpresaId, DateTime CompetenciaId)
        {
            List<Conta> contas = context.Contas.ToList();
            foreach (var conta in contas.Where(c => c.Codigo == 53))
            {
                var lancamentos = conta.Lancamentos.ToList();
                foreach (var lancamento in lancamentos.Where(l => l.EmpresaId == EmpresaId && l.DataCompetencia == CompetenciaId))
                {
                    ViewBag.Lancamentos = lancamento;
                }
            }
        }
    }
}
