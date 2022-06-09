using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Venda
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }
        public int EmpresaId { get; set; }

        [Required]
        public DateTime DataCompetencia { get; set; }
        [ForeignKey("DataCompetencia")]
        public Competencia Competencia { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string Observacao { get; set; }
        public List<ItemVenda> ItemVendas { get; set; }

    }
}
