using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demonstrativo.Models;

namespace Demonstrativo.Controllers
{
    public class OfxDescricoesController : Controller
    {
        private readonly Context _context;

        public OfxDescricoesController(Context context)
        {
            _context = context;
        }

        // GET: OfxDescricoes
        public async Task<IActionResult> Index()
        {
            var context = _context.OfxDescricoes.Include(o => o.ContaCredito).Include(o => o.ContaDebito);
            return View(await context.ToListAsync());
        }

        // GET: OfxDescricoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ofxDescricao = await _context.OfxDescricoes
                .Include(o => o.ContaCredito)
                .Include(o => o.ContaDebito)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ofxDescricao == null)
            {
                return NotFound();
            }

            return View(ofxDescricao);
        }

        // GET: OfxDescricoes/Create
        public IActionResult Create()
        {
            ViewData["ContaCreditoId"] = new SelectList(_context.ContasContabeis, "Codigo", "Codigo");
            ViewData["ContaDebitoId"] = new SelectList(_context.ContasContabeis, "Codigo", "Codigo");
            return View();
        }

        // POST: OfxDescricoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao,ContaDebitoId,ContaCreditoId")] OfxDescricao ofxDescricao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ofxDescricao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContaCreditoId"] = new SelectList(_context.ContasContabeis, "Codigo", "Codigo", ofxDescricao.ContaCreditoId);
            ViewData["ContaDebitoId"] = new SelectList(_context.ContasContabeis, "Codigo", "Codigo", ofxDescricao.ContaDebitoId);
            return View(ofxDescricao);
        }

        // GET: OfxDescricoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ofxDescricao = await _context.OfxDescricoes.FindAsync(id);
            if (ofxDescricao == null)
            {
                return NotFound();
            }
            ViewData["ContaCreditoId"] = new SelectList(_context.ContasContabeis, "Codigo", "Codigo", ofxDescricao.ContaCreditoId);
            ViewData["ContaDebitoId"] = new SelectList(_context.ContasContabeis, "Codigo", "Codigo", ofxDescricao.ContaDebitoId);
            return View(ofxDescricao);
        }

        // POST: OfxDescricoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,ContaDebitoId,ContaCreditoId")] OfxDescricao ofxDescricao)
        {
            if (id != ofxDescricao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ofxDescricao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfxDescricaoExists(ofxDescricao.Id))
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
            ViewData["ContaCreditoId"] = new SelectList(_context.ContasContabeis, "Codigo", "Codigo", ofxDescricao.ContaCreditoId);
            ViewData["ContaDebitoId"] = new SelectList(_context.ContasContabeis, "Codigo", "Codigo", ofxDescricao.ContaDebitoId);
            return View(ofxDescricao);
        }

        // GET: OfxDescricoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ofxDescricao = await _context.OfxDescricoes
                .Include(o => o.ContaCredito)
                .Include(o => o.ContaDebito)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ofxDescricao == null)
            {
                return NotFound();
            }

            return View(ofxDescricao);
        }

        // POST: OfxDescricoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ofxDescricao = await _context.OfxDescricoes.FindAsync(id);
            _context.OfxDescricoes.Remove(ofxDescricao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfxDescricaoExists(int id)
        {
            return _context.OfxDescricoes.Any(e => e.Id == id);
        }
    }
}
