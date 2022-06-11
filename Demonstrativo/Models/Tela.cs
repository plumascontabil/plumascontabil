using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demonstrativo.Models
{
    public class Tela
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(1000)")]
        public string nome { get; set; }
        public ICollection<RoleManager<IdentityRole>> Roles { get; set; }


        public void Qualquer() { }
    }
}
