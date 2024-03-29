﻿using Demonstrativo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Controllers
{
    public class LancamentosPadroesController : Controller
    {
        private readonly Context _context;

        public LancamentosPadroesController(Context context)
        {
            _context = context;
        }

        // GET: LancamentosPadroes
        public async Task<IActionResult> Index()
        {
            var context = _context.LancamentosPadroes.Include(l => l.Categoria).Include(l => l.ContaCredito).Include(l => l.ContaDebito).Include(l => l.Tipo);
            return View(await context.ToListAsync());
        }

        // GET: LancamentosPadroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lancamentoPadrao = await _context.LancamentosPadroes
                .Include(l => l.Categoria)
                .Include(l => l.ContaCredito)
                .Include(l => l.ContaDebito)
                .Include(l => l.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lancamentoPadrao == null)
            {
                return NotFound();
            }

            return View(lancamentoPadrao);
        }

        // GET: LancamentosPadroes/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descricao");
            ViewData["ContaCreditoId"] = new SelectList(_context.ContasContabeis, "Codigo", "Codigo");
            ViewData["ContaDebitoId"] = new SelectList(_context.ContasContabeis, "Codigo", "Codigo");
            ViewData["TipoContaId"] = new SelectList(_context.TiposContas, "Id", "Id");
            return View();
        }

        // POST: LancamentosPadroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Descricao,LancamentoHistorico,ContaDebitoId,ContaCreditoId,CategoriaId,TipoContaId")] LancamentoPadrao lancamentoPadrao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lancamentoPadrao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descricao", lancamentoPadrao.CategoriaId);
            ViewData["ContaCreditoId"] = new SelectList(_context.ContasContabeis, "Codigo", "Codigo", lancamentoPadrao.ContaCreditoId);
            ViewData["ContaDebitoId"] = new SelectList(_context.ContasContabeis, "Codigo", "Codigo", lancamentoPadrao.ContaDebitoId);
            ViewData["TipoContaId"] = new SelectList(_context.TiposContas, "Id", "Id", lancamentoPadrao.TipoContaId);
            return View(lancamentoPadrao);
        }

        // GET: LancamentosPadroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lancamentoPadrao = await _context.LancamentosPadroes.FindAsync(id);
            if (lancamentoPadrao == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descricao", lancamentoPadrao.CategoriaId);
            ViewData["ContaCreditoId"] = new SelectList(_context.ContasContabeis, "Codigo", "Codigo", lancamentoPadrao.ContaCreditoId);
            ViewData["ContaDebitoId"] = new SelectList(_context.ContasContabeis, "Codigo", "Codigo", lancamentoPadrao.ContaDebitoId);
            ViewData["TipoContaId"] = new SelectList(_context.TiposContas, "Id", "Id", lancamentoPadrao.TipoContaId);
            return View(lancamentoPadrao);
        }

        // POST: LancamentosPadroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Descricao,LancamentoHistorico,ContaDebitoId,ContaCreditoId,CategoriaId,TipoContaId")] LancamentoPadrao lancamentoPadrao)
        {
            if (id != lancamentoPadrao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lancamentoPadrao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LancamentoPadraoExists(lancamentoPadrao.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Descricao", lancamentoPadrao.CategoriaId);
            ViewData["ContaCreditoId"] = new SelectList(_context.ContasContabeis, "Codigo", "Codigo", lancamentoPadrao.ContaCreditoId);
            ViewData["ContaDebitoId"] = new SelectList(_context.ContasContabeis, "Codigo", "Codigo", lancamentoPadrao.ContaDebitoId);
            ViewData["TipoContaId"] = new SelectList(_context.TiposContas, "Id", "Id", lancamentoPadrao.TipoContaId);
            return View(lancamentoPadrao);
        }

        // GET: LancamentosPadroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lancamentoPadrao = await _context.LancamentosPadroes
                .Include(l => l.Categoria)
                .Include(l => l.ContaCredito)
                .Include(l => l.ContaDebito)
                .Include(l => l.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lancamentoPadrao == null)
            {
                return NotFound();
            }

            return View(lancamentoPadrao);
        }

        // POST: LancamentosPadroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lancamentoPadrao = await _context.LancamentosPadroes.FindAsync(id);
            _context.LancamentosPadroes.Remove(lancamentoPadrao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LancamentoPadraoExists(int id)
        {
            return _context.LancamentosPadroes.Any(e => e.Id == id);
        }
    }
}
