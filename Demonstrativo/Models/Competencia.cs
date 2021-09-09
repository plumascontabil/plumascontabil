using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Models
{
    public class Competencia
    {
        public DateTime Data { get; set; }
        public List<Lancamento> Lancamentos { get; set; }
    }
}
