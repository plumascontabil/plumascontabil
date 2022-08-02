using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class OfxBanco
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public int Codigo { get; set; }

        [Column(TypeName = "varchar(70)")]
        public string Nome { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string UrlImagem { get; set; }
        public List<OfxContaCorrente> ContasCorrentes { get; set; }
      
        [Column(TypeName = "varchar(20)")]
        public string CodigoContabil { get; set; }
    }
}
