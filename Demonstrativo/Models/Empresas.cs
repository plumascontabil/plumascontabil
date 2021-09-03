using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Models
{
    public class Empresas
    {
        public int EmpresaId { get; set; }
        public string RazaoSocial { get; set; }
        public int EmpresaCod { get; set; }

        public int LancamentoId { get; set; }
        public Lancamentos Lancamentos { get; set; }
    }
}
