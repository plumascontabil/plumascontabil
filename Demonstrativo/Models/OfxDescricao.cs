using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class OfxDescricao
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public  string Descricao { get; set; }
        [ForeignKey("ContaDebitoId")]
        public int ContaDebitoId { get; set; }
        public ContaContabil ContaDebito { get; set; }

        [ForeignKey("ContaCreditoId")]
        public int ContaCreditoId { get; set; }
        public ContaContabil ContaCredito{ get; set; }
        public List<OfxComplemento> ComplementosOfxs { get; set; }
    }
}
