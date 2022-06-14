using Demonstrativo.Models;
using DomainService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demonstrativo.Controllers
{
    public class BaseController : Controller
    {
        readonly Context _context;

        public BaseController(Context context)
        {
            _context = context;
        }

        protected void AdicionarCompetenciaMesAtual()
        {
            DateTime competenciaAtual = new(DateTime.Now.Year, DateTime.Now.Month, 01);

            var existe = _context.Competencias.Where(c => c.Data == competenciaAtual).FirstOrDefault();
            if (existe != null)
            {
                return;
            }

            var competencia = new Competencia()
            {
                Data = competenciaAtual
            };

            var result = _context.Competencias.Add(competencia);
            
            _context.SaveChanges();
        }

        protected DateTime ReturnCompetenciaMesAtual()
        {
            DateTime competenciaAtual = new(DateTime.Now.Year, DateTime.Now.Month, 01);

            var existe = _context.Competencias.Where(c => c.Data == competenciaAtual).FirstOrDefault();
            if (existe != null)
            {
                return existe.Data;
            }

            var competencia = new Competencia()
            {
                Data = competenciaAtual
            };

            var result = _context.Competencias.Add(competencia);

            _context.SaveChanges();

            return result.Entity.Data;
        }

        protected void CarregarEmpresasCompetencias(int? empresaId = null, DateTime? competenciasId = null)
        {

            if (empresaId.HasValue)
            {
                HttpContext.Session.SetInt32("empresaId", empresaId.Value);
            }
            else
            {
                try
                {
                    var aju = HttpContext.Session.GetInt32("empresaId");
                    empresaId = aju;
                    ViewBag.EmpresaSeleciodaId = empresaId.Value;
                }
                catch (Exception)
                {

                    empresaId = null;
                }

            }

            if (competenciasId.HasValue)
            {
                HttpContext.Session.SetString("competenciasId", competenciasId.Value.ToLongDateString());
            }
            else
            {

                try
                {
                    var aju = HttpContext.Session.GetString("competenciasId");
                    competenciasId = !string.IsNullOrEmpty(aju) ? Convert.ToDateTime(aju) : null;
                    ViewBag.CompetenciasSelecionadaId = competenciasId.Value.ToString("yyyy-MM-dd");
                }
                catch (Exception)
                {

                    competenciasId = null;
                }
            }


            List<Empresa> empresas = _context.Empresas.ToList();
            List<Competencia> competencias = _context.Competencias.ToList();


            ViewBag.CompetenciasId = new SelectList(
                    competencias.OrderByDescending(f => f.Data).Select(
                    c => new { Value = c.Data.ToShortDateString(), Text = c.Data.ToString("MM/yyyy") })
                    , "Value", "Text",
                    competenciasId.HasValue ? competenciasId.Value.ToShortDateString() : competenciasId);

            ViewBag.EmpresasId = new SelectList(empresas.Select(F => new { Value = F.Codigo, Text = $"{F.Codigo} - {F.RazaoSocial}" }).ToList(), "Value", "Text", empresaId);




        }
    }
}
