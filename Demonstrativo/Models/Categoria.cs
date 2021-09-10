using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Models
{
    public class Categoria
    {
        [Key]
        [Required]
        public int Codigo { get; set; }

        [Required]
        [Column(TypeName = "varchar(40)")]
        public string Descricao { get; set; }

        public List<Conta> Conta { get; set; }
    }
}
