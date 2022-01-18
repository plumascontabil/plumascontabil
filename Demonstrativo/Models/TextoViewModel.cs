using System;
using System.Collections.Generic;

namespace Demonstrativo.Models
{
    public class TextoViewModel
    {
        public DateTime? Data { get; set; }
        public int? CodigoContaDebito { get; set; }
        public int? CodigoContaCredito { get; set; }
        public decimal Valor { get; set; }
        public int? CodigoHistorico { get; set; }
        public string ComplementoHistorico { get; set; }
        public int IniciaLote { get; set; }
        public int? CodigoMatrizFilial { get; set; }
        public int CentroCustoDebito { get; set; }
        public int CentroCustoCredito { get; set; }
    }
}
