using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Demonstrativo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demonstrativo.Controllers
{
    public class BaseController : Microsoft.AspNetCore.Mvc.Controller
    {
        readonly Context _context;

        public BaseController(Context context)
        {
            _context = context;
        }

        protected void AdicionarCompetenciaMesAtual()
        {
            DateTime competenciaAtual = new(DateTime.Now.Year, DateTime.Now.Month, 01);

            if (_context.Competencias.Any(c => c.Data == competenciaAtual))
            {
                return;
            }

            var competencia = new Competencia()
            {
                Data = competenciaAtual
            };

            _context.Competencias.Add(competencia);
            _context.SaveChanges();
        }

        protected void CarregarEmpresasCompetencias(int? empresaId = null, DateTime? competenciasId = null)
        {
            List<Empresa> empresas = _context.Empresas.ToList();
            List<Competencia> competencias = _context.Competencias.ToList();


            ViewBag.CompetenciasId = new SelectList(
                    competencias.Select(
                    c => new { Value = c.Data.ToShortDateString(), Text = c.Data.ToString("MM/yyyy") })
                    , "Value", "Text",
                    competenciasId.HasValue ? competenciasId.Value.ToShortDateString() : competenciasId);

            ViewBag.EmpresasId = new SelectList(empresas.Select(F => new { Value = F.Codigo, Text = $"{F.Codigo} - {F.RazaoSocial}" }).ToList(), "Value", "Text", empresaId);
        }
    }
}
