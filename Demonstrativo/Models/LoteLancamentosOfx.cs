using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class LoteLancamentosOfx
    {
        [Key]
        public int Id { get; set; }
        public decimal Valor { get; set; }
        [Required]
        [Column(TypeName = "varchar(70)")]
        public string Descricao { get; set; }

        [ForeignKey("LancamentoOfxId")]
        public int LancamentoOfxId { get; set; }
        public LancamentoOfx LancamentoOfx { get; set; }
    }
}
