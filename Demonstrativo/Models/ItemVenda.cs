using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Models
{
    public class ItemVenda
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ForeignKey("VendaId")]
        public Venda Venda { get; set; }
        public int? vendaId { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }
        public int? ProdutoId { get; set; }

        public int Quatidade { get; set; }
    }
}
