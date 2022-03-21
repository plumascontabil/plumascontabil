using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class LancamentoPadrao
    {
        [Key]
        public int Id { get; set; }
        public int? Codigo { get; set; }

        [Required]
        [Column(TypeName = "varchar(70)")]
        public string Descricao { get; set; }
        public int? LancamentoHistorico { get; set; }

        [ForeignKey("ContaDebitoId")]
        public int? ContaDebitoId { get; set; }
        public ContaContabil ContaDebito { get; set; }

        [ForeignKey("ContaCreditoId")]
        public int? ContaCreditoId { get; set; }
        public ContaContabil ContaCredito { get; set; }

        [Required]
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId ")]
        public Categoria Categoria { get; set; }
        [ForeignKey("TipoContaId")]
        public TipoConta Tipo { get; set; }
        public int? TipoContaId { get; set; }
        public List<AutoDescricao> AutoDescricoes { get; set; }
        public List<OfxLancamento> OfxLancamentos { get; set; }
        public List<Lancamento> Lancamentos { get; set; }
    }
}
