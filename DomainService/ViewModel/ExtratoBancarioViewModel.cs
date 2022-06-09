
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DomainService.ViewModel
{
    public class ExtratoBancarioViewModel
    {
        public OfxContaCorrenteViewModel ContasCorrentes { get; set; }
        public OfxBancoViewModel Banco { get; set; }
        public SelectList Empresas { get; set; }
        public int EmpresaSelecionada { get; set; }
        public string Importado { get; set; }
        public OfxLancamentoManualViewModel LancamentoManual { get; set; }
    }
}
