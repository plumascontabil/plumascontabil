using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    [Table("RoleTela")]
    public class RoleTela
    {
        [Key]
        public int Id { get; set; }

        [Column("TelaId")]
        public int TelaId { get; set; }

        [Column("RoleId", TypeName = "varchar(450)")]
        public string RoleId { get; set; }

        public Tela Tela { get; set; }


    }
}
