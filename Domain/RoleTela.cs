using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class RoleTela
    {
        [Key]
        public int Id { get; set; }
        public int IdTela { get; set; }
        public Tela Tela { get; set; }

        public RoleManager<IdentityRole> IdRole { get; set; }
        public int Role { get; set; }

        public void Qualquer() { }
    }
}
