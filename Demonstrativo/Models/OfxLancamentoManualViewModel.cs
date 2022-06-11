using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace Demonstrativo.Models
{
    public class OfxLancamentoManualViewModel
    {
        public DateTime Data { get; set; }
        public SelectList Tipos { get; set; }
        public string TipoSelecionado { get; set; }
        public string Descricao { get; set; }

        [Range(0.0, 10000000000)]
        public decimal Valor { get; set; }

        public int ContaCreditar { get; set; }
        public int ContaDebitar { get; set; }

    }
}
