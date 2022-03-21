using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class ItemVenda
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("VendaId")]
        public Venda Venda { get; set; }
        public int VendaId { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }
        public int ProdutoId { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal Quantidade { get; set; }
        [Required]
        [Column(TypeName = "decimal(11,2)")]
        public decimal Preco { get; set; }
    }
}
