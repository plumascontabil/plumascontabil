using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class LancamentoOfx
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string Documento { get; set; }
        public string TipoLancamento { get; set; }
        [Column(TypeName = "varchar(150)")]
        public string Descricao { get; set; }

        [Required]
        [Column(TypeName = "decimal(11,2)")]
        public double ValorOfx { get; set; }
        public DateTime Data { get; set; }
        
        [ForeignKey("HistoricoOfxId")]
        public int HistoricoOfxId { get; set; }
        public HistoricoOfx HistoricoOfx { get; set; }
        
        [ForeignKey("ContaCorrenteId")]
        public int ContaCorrenteId { get; set; }
        public ContaCorrente ContaCorrente { get; set; }
    }
}
