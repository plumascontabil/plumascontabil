using System.Collections.Generic;

namespace DomainService.ViewModel
{
    public class ContaViewModel
    {
        public int Id { get; set; }
        public int? Codigo { get; set; }
        public string Descricao { get; set; }
        public List<LancamentoViewModel> Lancamentos { get; set; }
    }
}
