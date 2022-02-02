using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class HistoricoOfx
    {
        [Key]
        public int Id { get; set; }
        public  string Descricao { get; set; }        
        public int ContaDebitoId { get; set; }
        public ContaContabil ContaDebito { get; set; }            
        public int ContaCreditoId { get; set; }
        public ContaContabil ContaCredito{ get; set; }
        public List<ImportacaoOfx> ImportacoesOfx { get; set; }
    }
}
