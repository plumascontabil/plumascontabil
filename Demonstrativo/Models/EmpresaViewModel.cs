using System.Collections.Generic;

namespace Demonstrativo.Models
{
    public class EmpresaViewModel
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public string RazaoSocial { get; set; }
        public string Apelido { get; set; }
        public string Cnpj { get; set; }
        public string Situacao { get; set; }
    }
}
