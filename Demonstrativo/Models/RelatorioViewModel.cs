using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Demonstrativo.Models
{
    public class RelatorioViewModel
    {
        public SelectList Empresas { get; set; }
        public SelectList ContasContabeis { get; set; }
        public int Empresa { get; set; }
        public int RazaoEmpresa { get; set; }
        public int ContaContabil { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public double TransationValue { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public int ContaDebitar { get; set; }
        public int ContaCreditar { get; set; }
    }
}
