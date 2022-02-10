using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class BancoOfx
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [MaxLength(4)]
        public int Codigo { get; set; }

        [Column(TypeName = "varchar(70)")]
        public string Nome { get; set; }
        public List<ContaCorrente> ContasCorrentes { get; set; }
    }
}
