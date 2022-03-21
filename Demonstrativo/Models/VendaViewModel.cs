using System;
using System.Collections.Generic;

namespace Demonstrativo.Models
{
    public class VendaViewModel
    {
        public int Id { get; set; }
        public string Observacao { get; set; }
        public DateTime? Data { get; set; }
        public int? Empresa { get; set; }
        public List<ItemVendaViewModel> ItensVendas { get; set; }
        public List<ProdutoViewModel> Produtos { get; set; }
    }
}
