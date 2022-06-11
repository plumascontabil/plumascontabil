using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Demonstrativo.Models
{
    public class OfxLancamentoViewModel
    {
        public string Id { get; set; }
        public int HistoricoId { get; set; }
        public string Description { get; set; }
        public string Complemento { get; set; }
        public decimal TransationValue { get; set; }
        public DateTime Date { get; set; }
        public long CheckSum { get; set; }
        public string Type { get; set; }
        public SelectList LancamentosPadroes { get; set; }
        public int LancamentoPadraoSelecionado { get; set; }
        public SaldoMensalViewModel SaldoMensal { get; set; }

    }
}

