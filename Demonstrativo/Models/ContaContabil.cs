using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class ContaContabil
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Codigo { get; set; }
        public int Classificacao { get; set; }
        public string Historico { get; set; }        
        public List<OfxDescricao> HistoricosCreditosOfx { get; set; }
        public List<OfxDescricao> HistoricosDebitosOfx { get; set; }
        public List<OfxSaldoConta> SaldosContas { get; set; }
    }
}
