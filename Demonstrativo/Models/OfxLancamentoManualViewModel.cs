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
                var minus = !string.IsNullOrEmpty(ValorInput) && ValorInput.Contains('-');
                var value = string.IsNullOrEmpty(ValorInput) ? 0 : Convert.ToDecimal(ValorInput.Replace("-", "").Replace("R$", "").Trim());

                if (minus)
                {
                    value = value * -1;
                }
                return value;
            }
        }

        public int ContaCreditar { get; set; }
        public int ContaDebitar { get; set; }

    }
}
