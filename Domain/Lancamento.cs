using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Lancamento
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Descricao { get; set; }
        [Required]
        [Column(TypeName = "decimal(11,2)")]
        public decimal Valor { get; set; }
        public int? ContaId { get; set; }

        [ForeignKey("ContaId")]
        public LancamentoPadrao Conta { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataCompetencia { get; set; }

        [ForeignKey("DataCompetencia")]
        public Competencia Competencia { get; set; }
        [Required]
        public int EmpresaId { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }
    }
}
