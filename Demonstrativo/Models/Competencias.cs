using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Models
{
    public class Competencias
    {
        public DateTime CompetenciaId { get; set; }

        public int LancamentoId { get; set; }
        public Lancamentos Lancamentos { get; set; }
    }
}
