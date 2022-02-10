using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class Empresa
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Codigo { get; set; }
        [Required]
        [Column(TypeName = "varchar(150)")]
        public string RazaoSocial { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Apelido { get; set; }
        [Required]
        [Column(TypeName = "varchar(18)")]
        public string Cnpj { get; set; }

        [Column(TypeName = "varchar(2)")]
        public string Situacao { get; set; }
        public List<Lancamento> Lancamentos { get; set; }
        public List<ProvisoesDepreciacao> ProvisoesDepreciacoes { get; set; }
        public List<ContaCorrente> ContasCorrentes{ get; set; }
        public List<Venda> Vendas { get; set; }
    }
}
