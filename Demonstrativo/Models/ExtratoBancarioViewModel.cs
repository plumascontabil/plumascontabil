using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demonstrativo.Models
{
    public class ExtratoBancarioViewModel
    {
        public OfxContaCorrenteViewModel ContasCorrentes { get; set; }
        public OfxBancoViewModel Banco { get; set; }
        public SelectList Empresas { get; set; }
        public int EmpresaSelecionada { get; set; }
        public OfxLancamentoManualViewModel LancamentoManual { get; set; }
    }
}
