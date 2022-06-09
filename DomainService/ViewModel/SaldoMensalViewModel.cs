using System;

namespace DomainService.ViewModel
{
    public class SaldoMensalViewModel
    {
        public DateTime Competencia { get; set; }
        public decimal SaldoMensal { get; set; }
        public int ContaCorrenteId { get; set; }
    }
}
