using System;
using System.Collections.Generic;

namespace Demonstrativo.Models
{
    public class LancamentoViewModel
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public int Empresa { get; set; }
        public int? Conta { get; set; }
        public int? ContaCorrenteId { get; set; }
        public string Descricao { get; set; }
        public decimal Valor
        {
            get
            {
                var minus = !string.IsNullOrEmpty(ValorStr) && ValorStr.Contains('-');
                var value = string.IsNullOrEmpty(ValorStr) ? 0 : Convert.ToDecimal(ValorStr.Replace("-", "").Replace("R$", "").Trim());

                if (minus)
                {
                    value = value * -1;
                }
                return value;
            }
        }
        public string ValorStr { get; set; }
        public bool PodeDigitarDescricao { get; set; }
        public List<LancamentoViewModel> SaldoBancos { get; set; }
    }
}
