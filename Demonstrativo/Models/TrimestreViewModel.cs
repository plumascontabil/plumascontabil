using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

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
        public SelectList Empresas { get; set; }
        public int EmpresaSelecionada { get; set; }
        public SelectList Competencias { get; set; }
        public int CompetenciaSelecionada { get; set; }
    }
}
