using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    [Table("Tela")]
    public class Tela
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Nome { get; set; }

        public virtual IList<RoleTela> Roles { get; set; }
    }
}
