using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Models
{
    public class Conta
    {
        public int Codigo{ get; set; }
        public string Descricao { get; set; }
        public int LancamentoDebito { get; set; }
        public int LancamentoCredito { get; set; }
        public int LancamentoHistorico { get; set; }
        public bool LancamentoSomar { get; set; }
        public bool LancamentoExportaNo { get; set; }
        public bool LancamentoExportaYes { get; set; }
        public List<Lancamento> Lancamentos { get; set; }
    }
}
