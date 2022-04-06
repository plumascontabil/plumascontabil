using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class Categoria
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(40)")]
        public string Descricao { get; set; }
       
        public List<LancamentoPadrao> Conta { get; set; }
    }
}
