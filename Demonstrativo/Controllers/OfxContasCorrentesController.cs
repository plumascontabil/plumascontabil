using Demonstrativo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Controllers
{
    public class OfxContasCorrentesController : Controller
    {
        private readonly Context _context;

        public OfxContasCorrentesController(Context context)
        {
            _context = context;
        }

        // GET: OfxContasCorrentes
        public async Task<IActionResult> Index()
        {
            var context = _context.ContasCorrentes.Include(o => o.BancoOfx).Include(o => o.Empresa);
            return View(await context.ToListAsync());
        }

        // GET: OfxContasCorrentes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ofxContaCorrente = await _context.ContasCorrentes
                .Include(o => o.BancoOfx)
                .Include(o => o.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ofxContaCorrente == null)
            {
                return NotFound();
            }

            return View(ofxContaCorrente);
        }

        // GET: OfxContasCorrentes/Create
        public IActionResult Create()
        {
            ViewData["BancoOfxId"] = new SelectList(_context.OfxBancos, "Id", "Id");
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Codigo", "Apelido");
            return View();
        }

        // POST: OfxContasCorrentes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroConta,EmpresaId,BancoOfxId")] OfxContaCorrente ofxContaCorrente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ofxContaCorrente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BancoOfxId"] = new SelectList(_context.OfxBancos, "Id", "Id", ofxContaCorrente.BancoOfxId);
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Codigo", "Apelido", ofxContaCorrente.EmpresaId);
            return View(ofxContaCorrente);
        }

        // GET: OfxContasCorrentes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ofxContaCorrente = await _context.ContasCorrentes.FindAsync(id);
            if (ofxContaCorrente == null)
            {
                return NotFound();
            }
            ViewData["BancoOfxId"] = new SelectList(_context.OfxBancos, "Id", "Id", ofxContaCorrente.BancoOfxId);
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Codigo", "Apelido", ofxContaCorrente.EmpresaId);
            return View(ofxContaCorrente);
        }

        // POST: OfxContasCorrentes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroConta,EmpresaId,BancoOfxId")] OfxContaCorrente ofxContaCorrente)
        {
            if (id != ofxContaCorrente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ofxContaCorrente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfxContaCorrenteExists(ofxContaCorrente.Id))
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
            ViewData["BancoOfxId"] = new SelectList(_context.OfxBancos, "Id", "Id", ofxContaCorrente.BancoOfxId);
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Codigo", "Apelido", ofxContaCorrente.EmpresaId);
            return View(ofxContaCorrente);
        }

        // GET: OfxContasCorrentes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ofxContaCorrente = await _context.ContasCorrentes
                .Include(o => o.BancoOfx)
                .Include(o => o.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ofxContaCorrente == null)
            {
                return NotFound();
            }

            return View(ofxContaCorrente);
        }

        // POST: OfxContasCorrentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ofxContaCorrente = await _context.ContasCorrentes.FindAsync(id);
            _context.ContasCorrentes.Remove(ofxContaCorrente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfxContaCorrenteExists(int id)
        {
            return _context.ContasCorrentes.Any(e => e.Id == id);
        }
    }
}
