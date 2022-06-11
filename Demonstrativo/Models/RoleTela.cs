using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class RoleTela
    {
        [Key]
        public int Id { get; set; }
        public int IdTela { get; set; }
        public int IdRole { get; set; }
        public void Qualquer() { }
    }
}
