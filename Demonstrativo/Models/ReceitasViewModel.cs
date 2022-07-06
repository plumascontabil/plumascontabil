using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Models
{
    public class ReceitasViewModel
    {
        public int Id { get; set; }
        public int IdConta { get; set; }
        public int Codigo { get; set; }
        public string Conta { get; set; }
        public string Descricao { get; set; }
        public string ValorDebitoStr { get; set; }
        public string ValorCreditoStr { get; set; }
        public decimal? ValorDebito
        {
            get
            {
                return ValorDebitoStr != null ? Convert.ToDecimal(ValorDebitoStr.Replace("R$", string.Empty).Trim()) : null;
            }
        }
        public decimal? ValorCredito
        {
            get
            {
                return ValorCreditoStr != null ? Convert.ToDecimal(ValorCreditoStr.Replace("R$", string.Empty).Trim()) : null;
            }
        }

        public string TipoLancamento { get; set; }

        public int? Empresa { get; set; }
        public DateTime? Data { get; set; }
        public int TipoConta { get; set; }

    }
}
