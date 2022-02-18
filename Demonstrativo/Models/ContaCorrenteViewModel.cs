using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Demonstrativo.Models
{
    public class ContaCorrenteViewModel
    {
        public string ReturnUrl { get; set; }
        public int Id { get; set; }
        public string NumeroConta { get; set; }
        public int IdBanco { get; set; }
        public int IdEmpresa { get; set; }
        public int Banco { get; set; }
        public int Empresa { get; set; }
        public SelectList Empresas { get; set; }
        public SelectList Bancos { get; set; }
        public List<LancamentoOfxViewModel> LancamentosOfxs { get; set; }
    }
}
