using System.Collections.Generic;

namespace DomainService.ViewModel
{
    public class OfxContaCorrenteViewModel
    {
        public List<OfxLancamentoViewModel> OfxLancamentos { get; set; }
        public string NumeroConta { get; set; }
    }
}
