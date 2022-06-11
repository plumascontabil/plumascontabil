using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Tela
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(1000)")]
        public string nome { get; set; }

        public void Qualquer() { }
    }
}
