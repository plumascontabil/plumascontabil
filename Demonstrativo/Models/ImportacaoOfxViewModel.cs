using System;
using System.Collections.Generic;
using System.Linq;
using Demonstrativo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demonstrativo.Models
{
    public class ImportacaoOfxViewModel
    {
        public string Id { get; set; }
        public int? HistoricoId { get; set; }
        public string Description { get; set; }
        public double TransationValue { get; set; }
        public DateTime Date { get; set; }
        public long CheckSum { get; set; }
        public string Type { get; set; }
        public int BankCode { get; set; }
        public string BankAccountAgencyCode { get; set; }
        public string BankAccountAccountCode { get; set; }
        public string BankAccountType { get; set; }
        public string HeaderBankName { get; set; }
        public DateTime ServerDate { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
        public SelectList ContasDebitoContabeis { get; set; }
        public SelectList ContasCreditoContabeis { get; set; }
        public int ContasDebitoSelecionada { get; set; }
        public int ContasCreditoSelecionada { get; set; }

    }
}

