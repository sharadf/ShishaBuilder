using Microsoft.EntityFrameworkCore;
using ShishaBuilder.Core.Models;

namespace ShishaBuilder.Business.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Hookah> Hookahs { get; set; }
}