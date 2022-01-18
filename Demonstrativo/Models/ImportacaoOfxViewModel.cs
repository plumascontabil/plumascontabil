using System;

namespace Demonstrativo.Models
{
    public class ImportacaoOfxViewModel
    {

        public string Id { get; set; }
        public string Description { get; set; }
        public double TransationValue { get; set; }
        public DateTime Date { get; set; }
        public long CheckSum { get; set; }
        public string Type { get; set; }
    }
}
