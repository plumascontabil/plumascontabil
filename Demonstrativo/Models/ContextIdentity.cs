using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Demonstrativo.Models
{
    public class ContextIdentity : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ContextIdentity(DbContextOptions<ContextIdentity> options)
    : base(options)
        {
        }
    }
}
