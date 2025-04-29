using Microsoft.EntityFrameworkCore;
using ShishaBuilder.Core.Models;

namespace ShishaBuilder.Core.DB;

public class AppDbContext : DbContext
{
    public DbSet<Tobacco> Tobaccos { get; set; }
    public DbSet<Master> Masters { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderTobacco> OrderTobaccos { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options ): base(options){ }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Конфигурация Order -> OrderTobacco

        modelBuilder.Entity<OrderTobacco>()
            .Property(ot => ot.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderTobaccos)
            .WithOne(ot => ot.Order)
            .HasForeignKey(ot => ot.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Конфигурация OrderTobacco -> Tobacco
        modelBuilder.Entity<OrderTobacco>()
            .HasOne(ot => ot.Tobacco)
            .WithMany()
            .HasForeignKey(ot => ot.TobaccoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

