using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class ImportacaoOfx
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Documento { get; set; }
        public string TipoLancamento { get; set; }
        public string Descricao { get; set; }
        public double ValorOfx { get; set; }
        public DateTime Data { get; set; }
        
        [ForeignKey("HistoricoOfxId")]
        public int HistoricoOfxId { get; set; }
        public HistoricoOfx HistoricoOfx { get; set; }
        
        [Required]
        [ForeignKey("EmpresaId")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}
