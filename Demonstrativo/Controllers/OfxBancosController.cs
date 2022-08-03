﻿using Demonstrativo.Models;
using DomainService;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Demonstrativo.Controllers
{
    public class OfxBancosController : BaseController
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ILogger<OfxBanco> _logger;

        //private readonly OfxBancosDomainService _ofxBancosDomainService;


        public OfxBancosController(Context context, IWebHostEnvironment env,
            RoleManager<IdentityRole> roleManager,
            ILogger<OfxBanco> logger) : base(context, roleManager)
        {
            _context = context;
            _appEnvironment = env;
            _logger = logger;
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
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
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
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            return View();
        }

        // POST: OfxBancos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Nome,CodigoContabil")] OfxBanco ofxBanco, IFormFile file)
        {
            if (file == null)
            {
                ViewBag.Message = "Porfavor, Anexe a logo do banco";
            }
            else
            if (ModelState.IsValid)
            {


                if (file != null)
                {
                    if (!Directory.Exists($"{_appEnvironment.WebRootPath}\\Bancos"))
                    {
                        Directory.CreateDirectory($"{_appEnvironment.WebRootPath}\\Bancos");
                    }

                    string caminhoDestinoArquivo = $"{_appEnvironment.WebRootPath}\\Bancos\\{file.FileName}";

                    using (var stream = new FileStream(caminhoDestinoArquivo, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    ofxBanco.UrlImagem = $"~/Bancos/{caminhoDestinoArquivo.Split("\\").LastOrDefault()}";
                }

                //var ofxBanco = await _ofxBancosDomainService.CreateValidar(ofxBanco);
                _context.Add(ofxBanco);
                _logger.LogInformation(((int)EEventLog.Post), "Ofx Banco {ofxBanco} created.", ofxBanco.Id);

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
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
            if (id == null)
            {
                return NotFound();
            }

            var ofxBanco = await _context.OfxBancos.FindAsync(id);

            //ofxBanco.UrlImagem = $"~/Bancos/{ofxBanco.UrlImagem.Split("\\").LastOrDefault()}";
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Nome,CodigoContabil")] OfxBanco ofxBanco, IFormFile file)
        {
            var bank = _context.OfxBancos.SingleOrDefault(f => f.Id == id);

            if (id != ofxBanco.Id)
            {
                return NotFound();
            }


            bank.Id = ofxBanco.Id;
            bank.Codigo = ofxBanco.Codigo;
            bank.Nome = ofxBanco.Nome;
            bank.CodigoContabil = ofxBanco.CodigoContabil;
            if (ModelState.IsValid)
            {
                try
                {

                    if (file != null)
                    {
                        if (!Directory.Exists($"{_appEnvironment.WebRootPath}\\Bancos"))
                        {
                            Directory.CreateDirectory($"{_appEnvironment.WebRootPath}\\Bancos");
                        }

                        string caminhoDestinoArquivo = $"{_appEnvironment.WebRootPath}\\Bancos\\{file.FileName}";

                        using (var stream = new FileStream(caminhoDestinoArquivo, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        bank.UrlImagem = $"~/Bancos/{caminhoDestinoArquivo.Split("\\").LastOrDefault()}";
                    }
                    else
                    {
                        bank.UrlImagem = bank.UrlImagem;
                    }
                    //var ofxBanco = await _ofxBancosDomainService.EditValidar(ofxBanco);

                    _context.Update(bank);
                    _logger.LogInformation(((int)EEventLog.Put), "Ofx Banco {ofxBanco} edited.", ofxBanco.Id);
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
            AdicionarCompetenciaMesAtual();
            CarregarEmpresasCompetencias();
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
            _logger.LogInformation(((int)EEventLog.Delete), "Ofx Banco {ofxBanco} deleted.", ofxBanco.Id);
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
