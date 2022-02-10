using System.Collections.Generic;

namespace Demonstrativo.Models
{
    public class ContaCorrenteViewModel
    {
        public string NumeroConta { get; set; }
        public List<LancamentoOfxViewModel> LancamentosOfxs { get; set; }
    }
}
