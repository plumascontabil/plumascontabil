using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class UsuarioEmpresa
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(450)")]
        public string UsuarioId { get; set; }
        [Required]
        public int EmpresaId { get; set; }
        public List<Empresa> Empresa { get; set; }
    }
}
