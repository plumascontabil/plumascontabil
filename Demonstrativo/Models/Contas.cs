using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Models
{
    public class Contas
    {
        public int ContaID { get; set; }
        public int CodConta { get; set; }
        public int LancamentoDebito { get; set; }
        public int LancamentoCredito { get; set; }
        public int LancamentoHistorico { get; set; }
        public bool LancamentoSomar { get; set; }
        public bool LancamentoExportaNo { get; set; }
        public bool LancamentoExportaYes { get; set; }

        public int LancamentoId { get; set; }
        public Lancamentos Lancamentos { get; set; }
    }
}
