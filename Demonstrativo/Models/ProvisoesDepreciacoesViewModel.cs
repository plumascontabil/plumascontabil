using System;

namespace Demonstrativo.Models
{
    public class ProvisoesDepreciacoesViewModel
    {
        public int Id { get; set; }
        public decimal? DecimoTerceiro { get; set; }
        public decimal? Ferias { get; set; }
        public decimal? Depreciacao { get; set; }
        public decimal? SaldoPrejuizo { get; set; }
        public DateTime Data { get; set; }
        public int Empresa { get; set; }
        public bool CalcularCompesacao { get; set; }
        public bool Apurar { get; set; }

    }
}
