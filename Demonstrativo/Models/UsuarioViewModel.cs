using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demonstrativo.Models
{
    public class UsuarioViewModel
    {
        public string Id { get; set; }
        public string Usuario { get; set; }
        public string Email { get; set; }

        [Display(Name = "User Role")]
        public string UserRole { get; set; }
    }
}
