using System.Collections.Generic;

namespace Demonstrativo.Models
{
    public class OfxComplementoViewModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public List<OfxDescricaoViewModel> HistoricoOfxViewModel { get; set; }
        public List<OfxLancamentoViewModel> LancamentoOfx { get; set; }
    }
}
