using Demonstrativo.Models;
using DomainService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Controllers
{
    public class OfxBancosController : BaseController
    {
        private readonly Context _context;
        //private readonly OfxBancosDomainService _ofxBancosDomainService;


        public OfxBancosController(Context context
            //OfxBancosDomainService ofxBancosDomainService
            ) : base(context)
        {
            _context = context;
            //_ofxBancosDomainService = ofxBancosDomainService;
        }

        // GET: OfxBancos
        public async Task<IActionResult> Index()
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View(await _context.OfxBancos.ToListAsync());
        }

        // GET: OfxBancos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ofxBanco = await _context.OfxBancos
                .FirstOrDefaultAsync(m => m.Id == id);

            //var ofxBanco = await _ofxBancosDomainService.Details(id);
            if (ofxBanco == null)
            {
                return NotFound();
            }
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View(ofxBanco);
        }

        // GET: OfxBancos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OfxBancos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Nome")] OfxBanco ofxBanco)
        {
            if (ModelState.IsValid)
            {
                //var ofxBanco = await _ofxBancosDomainService.CreateValidar(ofxBanco);
                _context.Add(ofxBanco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View(ofxBanco);
        }

        // GET: OfxBancos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ofxBanco = await _context.OfxBancos.FindAsync(id);
            if (ofxBanco == null)
            {
                return NotFound();
            }
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View(ofxBanco);
        }

        // POST: OfxBancos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Nome")] OfxBanco ofxBanco)
        {
            if (id != ofxBanco.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //var ofxBanco = await _ofxBancosDomainService.EditValidar(ofxBanco);

                    _context.Update(ofxBanco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfxBancoExists(ofxBanco.Id))
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
            return View(ofxBanco);
        }

        // GET: OfxBancos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ofxBanco = await _context.OfxBancos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ofxBanco == null)
            {
                return NotFound();
            }
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View(ofxBanco);
        }

        // POST: OfxBancos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var ofxBanco = await _ofxBancosDomainService.DeleteConfirmado(id);

            var ofxBanco = await _context.OfxBancos.FindAsync(id);
            _context.OfxBancos.Remove(ofxBanco);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfxBancoExists(int id)
        {
            //var ofxBanco = await _ofxBancosDomainService.OfxBancoExists(id);

            return _context.OfxBancos.Any(e => e.Id == id);
        }
    }
}
