using System.Collections.Generic;

namespace Demonstrativo.Models
{
    public class ContaViewModel
    {
        public int Id { get; set; }
        public int? Codigo { get; set; }
        public string Descricao { get; set; }
        public string TipoLancamento { get; set; }
        public List<LancamentoViewModel> Lancamentos { get; set; }
    }
}
