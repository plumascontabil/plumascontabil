using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Models
{
    public class Lancamentos
    {
        public int LancamentoId { get; set; }
        public decimal Valor { get; set; }
        public List<Contas> ContaId { get; set; }
        public List<Competencias> CompetenciaId { get; set; }
        public List<Empresas> EmpresaId { get; set; }
    }
}
