using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Demonstrativo.Models
{
    public class DadosViewModel
    {
        public string Id { get; set; }
        public int HistoricoId { get; set; }
        public string Description { get; set; }
        public double TransationValue { get; set; }
        public DateTime Date { get; set; }
        public long CheckSum { get; set; }
        public string Type { get; set; }
        public SelectList ContasDebitoContabeis { get; set; }
        public SelectList ContasCreditoContabeis { get; set; }
        public int ContasDebitoSelecionada { get; set; }
        public int ContasCreditoSelecionada { get; set; }
    }
}
