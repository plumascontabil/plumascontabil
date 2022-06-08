using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class TipoConta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string Descricao { get; set; }
        public List<LancamentoPadrao> Contas { get; set; }
    }
}
