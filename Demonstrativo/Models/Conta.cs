using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Models
{
    public class Conta
    {
        [Key]
        public int Id { get; set; }

        public int? Codigo { get; set; }
        
        [Required]
        [Column(TypeName = "varchar(70)")]
        public string Descricao { get; set; }
        
        
        [MaxLength(5)]
        public int? LancamentoDebito { get; set; }
        
        
        [MaxLength(5)]
        public int? LancamentoCredito { get; set; }
        
        public int? LancamentoHistorico { get; set; }
        //public bool LancamentoSomar { get; set; }
        //public bool LancamentoExportaNo { get; set; }
        //public bool LancamentoExportaYes { get; set; }
        public List<Lancamento> Lancamentos { get; set; }
        [Required]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId ")]
        public Categoria Categoria { get; set; }

        //[ForeignKey("TipoContaId ")]
        //public TipoConta Tipo { get; set; }
        //public int? TipoContaId { get; set; }
    }
}
