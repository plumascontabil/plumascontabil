using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demonstrativo.Models
{
    public class RegistrarRoleTelaViewModel
    {

        [Required]
        public List<int> TelaId { get; set; }

        [Required]
        [Display(Name = "User Role")]
        public string Role { get; set; }

       
    }
}
