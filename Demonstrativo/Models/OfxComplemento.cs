using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class OfxComplemento
    {

        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(70)")]
        public string DescricaoComplemento { get; set; }
        [Required]
        public int HistoricoId { get; set; }
        [ForeignKey("HistoricoId")]
        public OfxDescricao HistoricoOfx { get; set; }
        [Required]
        public int OfxId { get; set; }

        [ForeignKey("OfxId")]
        public OfxLancamento LancamentoOfx { get; set; }
    }
}
