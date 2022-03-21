using System.Collections.Generic;

namespace Demonstrativo.Models
{
    public class OfxContaCorrenteViewModel
    {
        public List<OfxLancamentoViewModel> OfxLancamentos { get; set; }
        public string NumeroConta { get; set; }
    }
}
