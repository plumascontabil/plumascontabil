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

        public string ValorInput { get; set; }
        public decimal Valor
        {
            get
            {
                return ValorInput != null ? Convert.ToDecimal(ValorInput.Replace("R$", "").Trim()) : 0;
            }
        }

        public int ContaCreditar { get; set; }
        public int ContaDebitar { get; set; }

    }
}
