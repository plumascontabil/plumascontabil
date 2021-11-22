using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Models
{
    public class Competencia
    {
        [Key]
        [Required]
        public DateTime Data { get; set; }
        public List<Lancamento> Lancamentos { get; set; }
        public List<ProvisoesDepreciacao> ProvisoesDepreciacoes { get; set; }
    }
}
