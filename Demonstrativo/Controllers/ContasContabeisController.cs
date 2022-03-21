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
    public class ContasContabeisController : Controller
    {
        private readonly Context _context;

        public ContasContabeisController(Context context)
        {
            _context = context;
        }

        // GET: ContasContabeis
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContasContabeis.ToListAsync());
        }

        // GET: ContasContabeis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contaContabil = await _context.ContasContabeis
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (contaContabil == null)
            {
                return NotFound();
            }

            return View(contaContabil);
        }

        // GET: ContasContabeis/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContasContabeis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Classificacao,Historico")] ContaContabil contaContabil)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contaContabil);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contaContabil);
        }

        // GET: ContasContabeis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contaContabil = await _context.ContasContabeis.FindAsync(id);
            if (contaContabil == null)
            {
                return NotFound();
            }
            return View(contaContabil);
        }

        // POST: ContasContabeis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,Classificacao,Historico")] ContaContabil contaContabil)
        {
            if (id != contaContabil.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contaContabil);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContaContabilExists(contaContabil.Codigo))
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
            return View(contaContabil);
        }

        // GET: ContasContabeis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contaContabil = await _context.ContasContabeis
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (contaContabil == null)
            {
                return NotFound();
            }

            return View(contaContabil);
        }

        // POST: ContasContabeis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contaContabil = await _context.ContasContabeis.FindAsync(id);
            _context.ContasContabeis.Remove(contaContabil);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContaContabilExists(int id)
        {
            return _context.ContasContabeis.Any(e => e.Codigo == id);
        }
    }
}
