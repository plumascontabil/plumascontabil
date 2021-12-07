using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Models
{
    public class TrimestreViewModel
    {
        public List<LancamentoViewModel> LancamentosCompra { get; set; } = new List<LancamentoViewModel>();
        public List<LancamentoViewModel> LancamentosReceita { get; set; } = new List<LancamentoViewModel>();
        public List<LancamentoViewModel> LancamentosDespesa { get; set; } = new List<LancamentoViewModel>();
        public ProvisoesDepreciacoesViewModel ProvisoesDepreciacoes { get; set; } = new ProvisoesDepreciacoesViewModel();
        public VendaViewModel EstoqueVendas { get; set; } = new VendaViewModel();
        public int[] Trimestre { get; set; } = new int[] { };
        public List<CategoriaViewModel> Categorias { get; set; } = new List<CategoriaViewModel>();
    }
}
