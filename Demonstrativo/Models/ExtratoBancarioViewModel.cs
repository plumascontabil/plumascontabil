using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Demonstrativo.Models
{
    public class ExtratoBancarioViewModel
    {
        public OfxComplementoViewModel ComplementoOfxViewModel { get; set; }
        public OfxContaCorrenteViewModel ContasCorrentes { get; set; }
        public OfxBancoViewModel Banco { get; set; }
        public SelectList Empresas { get; set; }
        public int EmpresaSelecionada { get; set; }
        public OfxLancamentoManualViewModel LancamentoManual { get; set; }
    }
}
