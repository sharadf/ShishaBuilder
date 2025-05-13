using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShishaBuilder.Core.Models;

namespace ShishaBuilder.Core.Data
{
    public class AppDbContextIdentity : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public AppDbContextIdentity(DbContextOptions<AppDbContextIdentity> options)
            : base(options) { }
        public DbSet<Hookah> Hookahs { get; set; }
        public DbSet<Table> Tables { get; set; }
    }
}
