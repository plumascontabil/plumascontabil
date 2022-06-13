using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demonstrativo.Models
{
    public class RegistrarViewModel
    {
        public string ReturnUrl { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "User Role")]
        public List<IdentityRole> UserRoles { get; set; }

        [Required]
        [Display(Name = "Empresas")]
        public List<int> EmpresasId { get; set; }

        [Required]
        public string UserRole { get; set; }
    }
}
