using System.Collections.Generic;

namespace Demonstrativo.Models
{
    public class CategoriaViewModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public List<ContaViewModel> Contas { get; set; }
    }
}
