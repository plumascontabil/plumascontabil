namespace Demonstrativo.Models
{
    public class ItemVendaViewModel
    {
        public int Id { get; set; }
        public int VendaId { get; set; }
        public int ProdutoId { get; set; }
        public decimal Quantidade { get; set; }
        public decimal Preco { get; set; }
        public ProdutoViewModel Produto { get; set; }
    }
}
