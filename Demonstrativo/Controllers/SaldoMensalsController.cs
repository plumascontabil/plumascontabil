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
    public class SaldoMensalsController : Controller
    {
        private readonly Context _context;

        public SaldoMensalsController(Context context)
        {
            _context = context;
        }

        // GET: SaldoMensals
        public async Task<IActionResult> Index()
        {
            var context = _context.SaldoMensal.Include(s => s.ContaCorrente);
            return View(await context.ToListAsync());
        }

        // GET: SaldoMensals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saldoMensal = await _context.SaldoMensal
                .Include(s => s.ContaCorrente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (saldoMensal == null)
            {
                return NotFound();
            }

            return View(saldoMensal);
        }

        // GET: SaldoMensals/Create
        public IActionResult Create()
        {
            ViewData["ContaCorrenteId"] = new SelectList(_context.ContasCorrentes, "Id", "Id");
            return View();
        }

        // POST: SaldoMensals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Competencia,Saldo,ContaCorrenteId")] SaldoMensal saldoMensal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(saldoMensal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContaCorrenteId"] = new SelectList(_context.ContasCorrentes, "Id", "Id", saldoMensal.ContaCorrenteId);
            return View(saldoMensal);
        }

        // GET: SaldoMensals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saldoMensal = await _context.SaldoMensal.FindAsync(id);
            if (saldoMensal == null)
            {
                return NotFound();
            }
            ViewData["ContaCorrenteId"] = new SelectList(_context.ContasCorrentes, "Id", "Id", saldoMensal.ContaCorrenteId);
            return View(saldoMensal);
        }

        // POST: SaldoMensals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Competencia,Saldo,ContaCorrenteId")] SaldoMensal saldoMensal)
        {
            if (id != saldoMensal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(saldoMensal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaldoMensalExists(saldoMensal.Id))
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
            ViewData["ContaCorrenteId"] = new SelectList(_context.ContasCorrentes, "Id", "Id", saldoMensal.ContaCorrenteId);
            return View(saldoMensal);
        }

        // GET: SaldoMensals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var saldoMensal = await _context.SaldoMensal
                .Include(s => s.ContaCorrente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (saldoMensal == null)
            {
                return NotFound();
            }

            return View(saldoMensal);
        }

        // POST: SaldoMensals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var saldoMensal = await _context.SaldoMensal.FindAsync(id);
            _context.SaldoMensal.Remove(saldoMensal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaldoMensalExists(int id)
        {
            return _context.SaldoMensal.Any(e => e.Id == id);
        }
    }
}
