using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class ProvisoesDepreciacao
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Column(TypeName = "decimal(11,2)")]
        public decimal? DecimoTerceiro { get; set; }


        [Column(TypeName = "decimal(11,2)")]
        public decimal? Ferias { get; set; }


        [Column(TypeName = "decimal(11,2)")]
        public decimal? Depreciacao { get; set; }

        [Column(TypeName = "decimal(11,2)")]
        public decimal? SaldoPrejuizo { get; set; }
        public bool CalcularCompensacao { get; set; }
        public bool Apurar { get; set; }

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
