using Demonstrativo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using DomainService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Demonstrativo.Controllers
{
    public class ContasContabeisController : BaseController
    {
        private readonly Context _context;
        private readonly ILogger<ContaContabil> _logger;
        //private readonly ContasContabeisDomainService _contaContabilDomainService;


        public ContasContabeisController(Context context, 
            ILogger<ContaContabil> logger,
            RoleManager<IdentityRole> roleManager) : base(context, roleManager)
        //ContasContabeisDomainService contaContabilDomainService)
        {
            _context = context;
            _logger = logger;
            //_contaContabilDomainService = contaContabilDomainService;
        }

        // GET: ContasContabeis
        public async Task<IActionResult> Index()
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            //var contasContabeis = await _contaContabilDomainService.GetContasContabeis();
            return View(await _context.ContasContabeis.ToListAsync());
        }

        // GET: ContasContabeis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            if (id == null)
            {
                return NotFound();
            }

            //var contaContabil = _contaContabilDomainService.GetContaContavelById(id);
            var contaContabil = await _context.ContasContabeis
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (contaContabil == null)
            {
                return NotFound();
            }
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View(contaContabil);
        }

        // GET: ContasContabeis/Create
        public IActionResult Create()
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View();
        }

        // POST: ContasContabeis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Classificacao,Historico")] ContaContabil contaContabil)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            if (ModelState.IsValid)
            {
                //var contaContabil = _contaContabilDomainService.CreateValidar(contaContabil);
                _context.Add(contaContabil);
                await _context.SaveChangesAsync();
                _logger.LogInformation(((int)EEventLog.Post), "Conta Contabil Codigo: {codigo} created.", contaContabil.Codigo);
                return RedirectToAction(nameof(Index));
            }
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View(contaContabil);
        }

        // GET: ContasContabeis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            if (id == null)
            {
                return NotFound();
            }

            var contaContabil = await _context.ContasContabeis.FindAsync(id);
            if (contaContabil == null)
            {
                return NotFound();
            }
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View(contaContabil);
        }

        // POST: ContasContabeis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,Classificacao,Historico")] ContaContabil contaContabil)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            if (id != contaContabil.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //var contaContabil = _contaContabilDomainService.EditValidar(contaContabil);
                    _context.Update(contaContabil);
                    _logger.LogInformation(((int)EEventLog.Put), "Conta Contabil Codigo: {codigo} edited.", contaContabil.Codigo);
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
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View(contaContabil);
        }

        // GET: ContasContabeis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
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
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View(contaContabil);
        }

        // POST: ContasContabeis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var contaContabil = _contaContabilDomainService.DeleteConfirmado(id);
            var contaContabil = await _context.ContasContabeis.FindAsync(id);
            _context.ContasContabeis.Remove(contaContabil);
            _logger.LogInformation(((int)EEventLog.Delete), "Conta Contabil Codigo: {codigo} deleted.", contaContabil.Codigo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContaContabilExists(int id)
        {
            return _context.ContasContabeis.Any(e => e.Codigo == id);
        }
    }
}
