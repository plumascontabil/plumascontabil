using System.Collections.Generic;

namespace DomainService.ViewModel
{
    public class CategoriaViewModel
    {
        public string Descricao { get; set; }
        public List<ContaViewModel> Contas { get; set; }
    }
}
