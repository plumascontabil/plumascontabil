using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Models
{
    public class Empresa
    {
        public int Codigo { get; set; }
        public string RazaoSocial { get; set; }
        public string Apelido { get; set; }
        public int Cnpj { get; set; }
        public List<Lancamento> Lancamentos { get; set; }
    }
}
