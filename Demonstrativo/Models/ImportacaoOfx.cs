using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class ImportacaoOfx
    {
        [Key]
        public int Id { get; set; }
        public string TipoLancamento { get; set; }
        public decimal ValorOfx { get; set; }
        public DateTime Data { get; set; }
        [ForeignKey("HistoricoOfxId")]
        public int HistoricoOfxId { get; set; }
        public HistoricoOfx HistoricoOfx { get; set; }
    }
}
