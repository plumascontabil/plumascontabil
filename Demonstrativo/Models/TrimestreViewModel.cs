using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Models
{
    public class TrimestreViewModel
    {
        public List<LancamentoViewModel> LancamentosCompra { get; set; } = new List<LancamentoViewModel>();
        public List<CategoriaViewModel> Categorias { get; set; } = new List<CategoriaViewModel>();
    }
}
