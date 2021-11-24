using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Models
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
