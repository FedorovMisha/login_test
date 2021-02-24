using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccountWeb.Models
{
    public class LiteIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public LiteIdentityDbContext(DbContextOptions<LiteIdentityDbContext> options) : base(options)
        {
        }
    }
}