using Microsoft.EntityFrameworkCore;
using ShishaBuilder.Core.Models;

namespace ShishaBuilder.Core.DB;

public class AppDbContext :DbContext
{
    public DbSet<Tobacco> Tobaccos { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options ): base(options){ }
}

