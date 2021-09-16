using Demonstrativo.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Controllers
{
    public class LancamentoController : Controller
    {
        Context context = new Context();

        public IActionResult Index()
        {
            List<Lancamento> lancamentos = context.Lancamentos.ToList();
            List<Conta> contas = context.Contas.ToList();
            List<Categoria> categorias = context.Categorias.ToList();

            ViewBag.Contas = contas;
            ViewBag.Lancamentos = lancamentos;
            ViewBag.Categorias = categorias;

            return View();
        }
    }
}
