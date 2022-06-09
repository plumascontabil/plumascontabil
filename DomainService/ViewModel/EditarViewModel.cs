using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DomainService.ViewModel
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

        [Required]
        public string UserRole { get; set; }
    }
}
