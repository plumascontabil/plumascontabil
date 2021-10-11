using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Models
{
    public class LancamentoViewModel
    {
        public int Id { get; set; }
        public DateTime DataCompetencia { get; set; }
        public int EmpresaId { get; set; }
        public int? ContaId { get; set; }
        public string? Descricao { get; set; }
        public decimal Valor { get; set; }
        public bool PodeDigitarDescricao { get; set; }
    }
}
