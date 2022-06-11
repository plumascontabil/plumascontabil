using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class AutoDescricao
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(1000)")]
        public string Descricao { get; set; }

        [ForeignKey("LancamentoPadraoId")]
        public int LancamentoPadraoId { get; set; }
        public LancamentoPadrao LancamentoPadrao { get; set; }
        public void Qualquer() { }
    }
}
