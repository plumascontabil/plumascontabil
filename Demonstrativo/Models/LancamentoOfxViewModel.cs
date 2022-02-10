using System;
using System.Collections.Generic;
using System.Linq;
using Demonstrativo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demonstrativo.Models
{
    public class LancamentoOfxViewModel
    {
        public string Id { get; set; }
        public int HistoricoId { get; set; }
        public string Description { get; set; }
        public double TransationValue { get; set; }
        public DateTime Date { get; set; }
        public long CheckSum { get; set; }
        public string Type { get; set; }
        public SelectList ContasDebito { get; set; }
        public SelectList ContasCredito { get; set; }
        public int ContaDebitoSelecionada { get; set; }
        public int ContaCreditoSelecionada { get; set; }
        public List<HistoricoOfxViewModel> Historicos { get; set; }

    }
}

