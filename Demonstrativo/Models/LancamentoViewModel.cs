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
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public bool PodeDigitarDescricao { get; set; }
        public List<LancamentoViewModel> SaldoBancos { get; set; }
    }
}
