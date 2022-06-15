using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demonstrativo.Models
{
    public class EditarViewModel
    {
        public string Id { get; set; }
        public string ReturnUrl { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Display(Name = "User Role")]
        public List<IdentityRole> UserRoles { get; set; }

        //[Required]
        //[Display(Name = "Empresas")]
        //public List<int> EmpresasId { get; set; }

        [Required]
        public string UserRole { get; set; }
    }
}
