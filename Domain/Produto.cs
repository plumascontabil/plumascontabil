using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Produto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string Nome { get; set; }
        public List<ItemVenda> ItemVendas { get; set; }
    }
}
