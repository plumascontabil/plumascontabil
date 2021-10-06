using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Models
{
    public class ContaViewModel
    {
        public int Id { get; set; }
        public int? Codigo { get; set; }
        public string Descricao { get; set; }
        public List<LancamentoViewModel> Lancamentos { get; set; }
    }
}
