using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class SaldoMensal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Competencia { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Saldo { get; set; }

        [Required]
        [ForeignKey("ContaCorrete")]
        public int ContaCorrenteId { get; set; }
        public OfxContaCorrente ContaCorrente { get; set; }
    }
}
