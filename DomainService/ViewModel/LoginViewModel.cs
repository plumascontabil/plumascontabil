using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DomainService.ViewModel
{
    public class LoginViewModel
    {
        [TempData]
        public string ErrorMessage { get; set; }
        public string ReturnUrl { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
