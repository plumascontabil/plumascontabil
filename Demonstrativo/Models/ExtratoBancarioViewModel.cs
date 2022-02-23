using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Demonstrativo.Models
{
    public class ExtratoBancarioViewModel
    {
        public ContaCorrenteViewModel ContasCorrentes { get; set; }
        public BancoViewModel Banco { get; set; }
        public SelectList Empresas { get; set; }
        public int EmpresaSelecionada { get; set; }
        public LancamentoManualViewModel LancamentoManual { get; set; }
    }
}
