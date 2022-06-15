using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class OfxSaldoConta
    {
        [Key]
        public int Id { get; set; }
        public DateTime DataFim { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Saldo { get; set; }
        [Required]
        public int ContaContabilId { get; set; }
        [ForeignKey("ContaContabilId")]
        public ContaContabil ContaContabil { get; set; }
        [Required]
        public int EmpresaId { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }
    }
}
