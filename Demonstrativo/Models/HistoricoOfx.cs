using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class HistoricoOfx
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(40)")]
        public  string Descricao { get; set; }
        [ForeignKey("ContaDebitoId")]
        public int ContaDebitoId { get; set; }
        public ContaContabil ContaDebito { get; set; }

        [ForeignKey("ContaCreditoId")]
        public int ContaCreditoId { get; set; }
        public ContaContabil ContaCredito{ get; set; }
        public List<LancamentoOfx> ImportacoesOfx { get; set; }
    }
}
