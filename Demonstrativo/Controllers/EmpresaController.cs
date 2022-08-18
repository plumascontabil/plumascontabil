using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demonstrativo.Models;
using DomainService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PagedList;

namespace Demonstrativo.Controllers
{
    public class EmpresaController : BaseController
    {
        private readonly Context _context;
        private readonly ILogger<Empresa> _logger;
        //private readonly EmpresasDomainService _EmpresasDomainService;

        public EmpresaController(Context context,
            RoleManager<IdentityRole> roleManager,
            ILogger<Empresa> logger) : base(context, roleManager)

        {
            _context = context;
            _logger = logger;
            //_EmpresasDomainService = EmpresasDomainService;
        }
        public async Task<IActionResult> Index(string searchString, int? page)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            var context = await _context.Empresas.ToListAsync();
            if (searchString != null)
            {
                page = 1;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                context = context.Where(s => s.Apelido.Contains(searchString.ToUpper())
                                       || s.RazaoSocial.Contains(searchString.ToUpper())).ToList();
            }
            //int pageSize = 10;
            //int pageNumber = (page ?? 1);
            return View(context/*.ToPagedList(pageNumber, pageSize)*/);
        }

        // GET: Empresas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            if (id == null)
            {
                return NotFound();
            }

            //var Empresa = await _EmpresasDomainService.Details(id);
            var Empresa = await _context.Empresas
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (Empresa == null)
            {
                return NotFound();
            }
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View(Empresa);
        }

        // GET: Empresas/Create
        public IActionResult Create()
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View();
        }

        // POST: Empresas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,RazaoSocial,Apelido,Cnpj,Situacao")] Empresa empresa)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_context.Empresas.SingleOrDefault(f => f.Codigo == empresa.Codigo) != null)
                    {
                        AdicionarCompetenciaMesAtual();
                        CarregarEmpresasCompetencias();
                        TempData["erro"] = "Já existe empresa cadastrada com esse Número Informado.";
                        return View(empresa);

                    }


                    //var Empresa = await _EmpresasDomainService.CreateValidar(Empresa);
                    empresa.Situacao = "A";
                    _context.Add(empresa);
                    _logger.LogInformation(((int)EEventLog.Post), "Empresa Id: {id} created.", empresa.Codigo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                AdicionarCompetenciaMesAtual();
                CarregarEmpresasCompetencias();
                TempData["criado"] = "criado";
                return RedirectToAction("Details", empresa);
            }
            catch (Exception e)
            {
                AdicionarCompetenciaMesAtual();
                CarregarEmpresasCompetencias();
                TempData["erro"] = "erro";
                return View(empresa);
            }
        }

        // GET: Empresas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View(empresa);
        }

        // POST: Empresas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,RazaoSocial,Apelido,Cnpj,Situacao")] Empresa empresa)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var empresaDeletada = await _context.Empresas.FindAsync(id);
                    _context.Empresas.Remove(empresaDeletada);
                    //var Empresa = await _EmpresasDomainService.EditValidar(Empresa);
                    _context.Add(empresa);
                    _logger.LogInformation(((int)EEventLog.Put), "Empresa Id: {id} edited.", empresa.Codigo);
                    await _context.SaveChangesAsync();
                    TempData["editado"] = "editado";

                    AdicionarCompetenciaMesAtual();
                    CarregarEmpresasCompetencias();

                    return View(empresa);
                }
                AdicionarCompetenciaMesAtual();
                CarregarEmpresasCompetencias();
                return View(empresa);
            }
            catch (Exception e)
            {
                AdicionarCompetenciaMesAtual();
                CarregarEmpresasCompetencias();
                TempData["erro"] = "erro";
                return View(empresa);
            }
        }

        // GET: Empresas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            if (id == null)
            {
                return NotFound();
            }

            var Empresa = await _context.Empresas
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (Empresa == null)
            {
                return NotFound();
            }
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View(Empresa);
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                //var Empresa = await _EmpresasDomainService.DeleteConfirmado(id);

                var empresa = await _context.Empresas.FindAsync(id);
                _context.Empresas.Remove(empresa);
                _logger.LogInformation(((int)EEventLog.Put), "Empresa Id: {id} deleted.", empresa.Codigo);
                await _context.SaveChangesAsync();
                TempData["deletado"] = "deletado";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                AdicionarCompetenciaMesAtual();
                CarregarEmpresasCompetencias();
                TempData["erro"] = "erro";
                var empresa = await _context.Empresas.FindAsync(id);
                return View(empresa);
            }
        }

        private bool EmpresaExists(int id)
        {
            //var Empresa = await _EmpresasDomainService.EmpresaExists(id);

            return _context.Empresas.Any(e => e.Codigo == id);
        }
    }
}
