using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Demonstrativo.Models
{
    public class HistoricoOfxViewModel
    {
        public string ReturnUrl { get; set; }
        public int Id { get; set; }
        public string Descricao { get; set; }
        public SelectList ContaDebitoId { get; set; }
        public SelectList ContaCreditoId { get; set; }
        public int CodigoContaDebitoSelecionada { get; set; }
        public int CodigoContaCreditoSelecionada { get; set; }
    }
}
