using System;

namespace Demonstrativo.Models
{
    public class ItemVendaViewModel
    {
        public int Id { get; set; }
        public int VendaId { get; set; }
        public int ProdutoId { get; set; }
        public decimal Quantidade { get; set; }
        public string PrecoVlr { get; set; }
        public decimal Preco
        {
            get
            {
                return Convert.ToDecimal(PrecoVlr.Replace("R$", "").Trim());
            }
        }
        public ProdutoViewModel Produto { get; set; }
    }
}
