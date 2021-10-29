using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Models
{
    public class TrimestreViewModel
    {
        public List<Lancamento> ComprasTrimestre { get; set; }
        public List<CategoriaViewModel>? Categorias { get; set; }
    }
}
