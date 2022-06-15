using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class OfxLoteLancamento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        [Required]
        [Column(TypeName = "varchar(70)")]
        public string Descricao { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime Data { get; set; }

        [ForeignKey("EmpresaId")]
        public int EmpresaId { get; set; }
        public DateTime CompetenciaId { get; set; }
        public virtual List<OfxLancamento> Lancamentos { get; set; }
        public Empresa Empresa { get; set; }
    }
}
