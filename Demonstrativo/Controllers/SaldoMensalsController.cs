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
using Microsoft.Extensions.Logging;

namespace Demonstrativo.Controllers
{
    public class SaldoMensalsController : BaseController
    {
        private readonly Context _context;
        private readonly ILogger<SaldoMensal> _logger;
        //private readonly SaldoMensalsDomainService _saldoMensalsDomainService;

        public SaldoMensalsController(Context context,
            RoleManager<IdentityRole> roleManager,
            ILogger<SaldoMensal> logger) : base(context,roleManager)

        {
            _context = context;
            _logger = logger;
            //_saldoMensalsDomainService = saldoMensalsDomainService;
        }

        // GET: SaldoMensals
        public async Task<IActionResult> Index()
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            var context = _context.SaldoMensal.Include(s => s.ContaCorrente);
            return View(await context.ToListAsync());
        }

        // GET: SaldoMensals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            if (id == null)
            {
                return NotFound();
            }

            //var saldoMensal = await _saldoMensalsDomainService.Details(id);
            var saldoMensal = await _context.SaldoMensal
                .Include(s => s.ContaCorrente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (saldoMensal == null)
            {
                return NotFound();
            }
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View(saldoMensal);
        }

        // GET: SaldoMensals/Create
        public IActionResult Create()
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
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
                //var saldoMensal = await _saldoMensalsDomainService.CreateValidar(saldoMensal);
                _context.Add(saldoMensal);
                _logger.LogInformation(((int)EEventLog.Post), "Saldo Mensal Id: {id} created.", saldoMensal.Id);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            ViewData["ContaCorrenteId"] = new SelectList(_context.ContasCorrentes, "Id", "Id", saldoMensal.ContaCorrenteId);
            return View(saldoMensal);
        }

        // GET: SaldoMensals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            if (id == null)
            {
                return NotFound();
            }

            var saldoMensal = await _context.SaldoMensal.FindAsync(id);
            if (saldoMensal == null)
            {
                return NotFound();
            }
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
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
                    //var saldoMensal = await _saldoMensalsDomainService.EditValidar(saldoMensal);
                    _context.Update(saldoMensal);
                    _logger.LogInformation(((int)EEventLog.Put), "Saldo Mensal Id: {id} edited.", saldoMensal.Id);
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
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            ViewData["ContaCorrenteId"] = new SelectList(_context.ContasCorrentes, "Id", "Id", saldoMensal.ContaCorrenteId);
            return View(saldoMensal);
        }

        // GET: SaldoMensals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
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
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View(saldoMensal);
        }

        // POST: SaldoMensals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var saldoMensal = await _saldoMensalsDomainService.DeleteConfirmado(id);

            var saldoMensal = await _context.SaldoMensal.FindAsync(id);
            _context.SaldoMensal.Remove(saldoMensal);
            _logger.LogInformation(((int)EEventLog.Put), "Saldo Mensal Id: {id} deleted.", saldoMensal.Id);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaldoMensalExists(int id)
        {
            //var saldoMensal = await _saldoMensalsDomainService.SaldoMensalExists(id);

            return _context.SaldoMensal.Any(e => e.Id == id);
        }
    }
}
