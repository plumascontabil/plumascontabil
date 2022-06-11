
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repository.Contexts
{
    public class ContextIdentity : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ContextIdentity(DbContextOptions<ContextIdentity> options)
    : base(options)
        {
        }
    }
}
