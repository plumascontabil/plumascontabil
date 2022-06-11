using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class OfxLancamento
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public DateTime Data { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string Documento { get; set; }
        public string Descricao { get; set; }

        [Required]
        [Column(TypeName = "decimal(11,2)")]
        public decimal ValorOfx { get; set; }

        [ForeignKey("ContaCorrenteId")]
        public int ContaCorrenteId { get; set; }
        public OfxContaCorrente ContaCorrente { get; set; }
        public string TipoLancamento { get; set; }
        //public List<OfxLoteLancamento> LoteLancamentos { get; set; }
        [ForeignKey("LancamentoPadraoId")]
        public int? LancamentoPadraoId { get; set; }
        public LancamentoPadrao LancamentoPadrao { get; set; }
        [ForeignKey("LancamentoPadraoId")]
        public int? LoteLancamentoId { get; set; }
        public OfxLoteLancamento Lote { get; set; }
    }
}
