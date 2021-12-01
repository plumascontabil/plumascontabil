using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Demonstrativo.Models
{
    public class Venda
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }
        public int? EmpresaId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [ForeignKey("DataCompetencia")]
        public DateTime DataCompetencia { get; set; }
        public Competencia Competencia { get; set; }

        public string? Observacao { get; set; }

        public List<ItemVenda> ItemVendas { get; set; }
    }
}
