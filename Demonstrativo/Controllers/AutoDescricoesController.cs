using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demonstrativo.Models;
using DomainService;
using Microsoft.AspNetCore.Identity;

namespace Demonstrativo.Controllers
{
    public class AutoDescricoesController : BaseController
    {
        private readonly Context _context;
        //private readonly AutoDescricoesDomainService _autoDescricoesDomainService;


        public AutoDescricoesController(
            Context context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager

            //AutoDescricoesDomainService autoDescricoesDomainService
            ) : base(context, roleManager)
        {
            _context = context;
            //_autoDescricoesDomainService = autoDescricoesDomainService;
        }

        // GET: AutoDescricoes
        public async Task<IActionResult> Index()
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            var context = _context.AutoDescricoes.Include(a => a.LancamentoPadrao);
            //var context = _autoDescricoesDomainService.AutoDescricoes();
            return View(await context.ToListAsync());
        }

        // GET: AutoDescricoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            if (id == null)
            {
                return NotFound();
            }

            var autoDescricao = await _context.AutoDescricoes
                .Include(a => a.LancamentoPadrao)
                .FirstOrDefaultAsync(m => m.Id == id);
            //var autoDescricao = await _autoDescricoesDomainService.GetAutoDescricaoById(id);
            if (autoDescricao == null)
            {
                return NotFound();
            }
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View(autoDescricao);
        }

        // GET: AutoDescricoes/Create
        public IActionResult Create()
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            ViewData["LancamentoPadraoId"] = new SelectList(_context.LancamentosPadroes, "Id", "Descricao");
            return View();
        }

        // POST: AutoDescricoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao,LancamentoPadraoId")] AutoDescricao autoDescricao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autoDescricao);
                await _context.SaveChangesAsync();
                //await _autoDescricoesDomainService.CreateValidar(autoDescricao);
                return RedirectToAction(nameof(Index));
            }
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            ViewData["LancamentoPadraoId"] = new SelectList(_context.LancamentosPadroes, "Id", "Descricao", autoDescricao.LancamentoPadraoId);
            return View(autoDescricao);
        }

        // GET: AutoDescricoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            if (id == null)
            {
                return NotFound();
            }

            var autoDescricao = await _context.AutoDescricoes.FindAsync(id);
            if (autoDescricao == null)
            {
                return NotFound();
            }
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            ViewData["LancamentoPadraoId"] = new SelectList(_context.LancamentosPadroes, "Id", "Descricao", autoDescricao.LancamentoPadraoId);
            return View(autoDescricao);
        }

        // POST: AutoDescricoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,LancamentoPadraoId")] AutoDescricao autoDescricao)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            if (id != autoDescricao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autoDescricao);
                    await _context.SaveChangesAsync();
                    //await _autoDescricoesDomainService.EditValidar(autoDescricao);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutoDescricaoExists(autoDescricao.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            ViewData["LancamentoPadraoId"] = new SelectList(_context.LancamentosPadroes, "Id", "Descricao", autoDescricao.LancamentoPadraoId);
            return View(autoDescricao);
        }

        // GET: AutoDescricoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            if (id == null)
            {
                return NotFound();
            }

            var autoDescricao = await _context.AutoDescricoes
                .Include(a => a.LancamentoPadrao)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (autoDescricao == null)
            {
                return NotFound();
            }
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View(autoDescricao);
        }

        // POST: AutoDescricoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var autoDescricao = await _context.AutoDescricoes.FindAsync(id);
            _context.AutoDescricoes.Remove(autoDescricao);
            await _context.SaveChangesAsync();
            //_autoDescricoesDomainService.DeleteConfirmado(id);
            return RedirectToAction(nameof(Index));
        }

        private bool AutoDescricaoExists(int id)
        {
            return _context.AutoDescricoes.Any(e => e.Id == id);
        }
    }
}
