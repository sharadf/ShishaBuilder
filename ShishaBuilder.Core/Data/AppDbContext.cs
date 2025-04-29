using Microsoft.EntityFrameworkCore;
using ShishaBuilder.Core.Models;

namespace ShishaBuilder.Core.Data;

public class AppDbContext : DbContext   
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Hookah> Hookahs { get; set; }
    public DbSet<Table> Tables { get; set; }
}
