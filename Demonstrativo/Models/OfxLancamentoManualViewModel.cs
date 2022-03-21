using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Demonstrativo.Models
{
    public class OfxLancamentoManualViewModel
    {
        public DateTime Data { get; set; }
        public SelectList Tipos { get; set; }
        public string TipoSelecionado { get; set; }
        public string Descricao { get; set; }
        public double Valor { get; set; }
        public int ContaCreditar { get; set; }
        public int ContaDebitar { get; set; }

    }
}
