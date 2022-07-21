using System;

namespace Demonstrativo.Models
{
    public class ProvisoesDepreciacoesViewModel
    {
        public int Id { get; set; }
        public decimal? DecimoTerceiro
        {
            get
            {
                return string.IsNullOrEmpty(DecimoTerceiroVlr) ? null : Convert.ToDecimal(DecimoTerceiroVlr.Replace("R$", "").Trim());
            }
        }

        public string DecimoTerceiroVlr { get; set; }
        public decimal? Ferias
        {
            get
            {
                return string.IsNullOrEmpty(FeriasVlr) ? null : Convert.ToDecimal(FeriasVlr.Replace("R$", "").Trim());
            }
        }
        public string FeriasVlr { get; set; }

        public decimal? Depreciacao
        {
            get
            {
                return string.IsNullOrEmpty(DepreciacaoVlr) ? null : Convert.ToDecimal(DepreciacaoVlr.Replace("R$", "").Trim());
            }
        }
        public string DepreciacaoVlr { get; set; }
        public decimal? SaldoPrejuizo
        {
            get
            {
                return string.IsNullOrEmpty(SaldoPrejuizoVlr) ? null : Convert.ToDecimal(SaldoPrejuizoVlr.Replace("R$", "").Trim());
            }
        }
        public string SaldoPrejuizoVlr { get; set; }

        public decimal? CompesacaoPrejuizo
        {
            get
            {
                return string.IsNullOrEmpty(CompesacaoPrejuizoVlr) ? null : Convert.ToDecimal(CompesacaoPrejuizoVlr.Replace("R$", "").Trim());
            }
        }

        public string CompesacaoPrejuizoVlr { get; set; }
        public DateTime Data { get; set; }
        public int Empresa { get; set; }
        public bool CalcularCompesacao { get; set; }
        public bool Apurar { get; set; }

    }
}
