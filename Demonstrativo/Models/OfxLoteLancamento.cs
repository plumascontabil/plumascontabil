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
        public int Id { get; set; }
        public decimal Valor { get; set; }

        [Required]
        [Column(TypeName = "varchar(70)")]
        public string Descricao { get; set; }

        [Required]
        public DateTime Data { get; set; }

        public virtual ICollection<OfxLancamento> Lancamentos { get; set; }
    }
}
