// using System.Reflection.Emit;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore;
// using ShishaBuilder.Core.Models;

// namespace ShishaBuilder.Core.Data
// {
//     public class AppDbContextIdentity : IdentityDbContext<AppUser, IdentityRole, string>
//     {
//         public AppDbContextIdentity(DbContextOptions<AppDbContextIdentity> options)
//             : base(options) { }

//         public DbSet<Hookah> Hookahs { get; set; }
//         public DbSet<Table> Tables { get; set; }
//         public DbSet<Tobacco> Tobaccos { get; set; }
//         public DbSet<Master> Masters { get; set; }
//         public DbSet<Order> Orders { get; set; }
//         public DbSet<OrderTobacco> OrderTobaccos { get; set; }

//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//         {
//             base.OnModelCreating(modelBuilder);

//             modelBuilder
//                 .Entity<Master>()
//                 .HasOne(m => m.AppUser)
//                 .WithMany()
//                 .HasForeignKey(m => m.AppUserId)
//                 .OnDelete(DeleteBehavior.Cascade);

//             modelBuilder.Entity<OrderTobacco>().Property(ot => ot.Id).ValueGeneratedOnAdd();

//             modelBuilder
//                 .Entity<Order>()
//                 .HasMany(o => o.OrderTobaccos)
//                 .WithOne(ot => ot.Order)
//                 .HasForeignKey(ot => ot.OrderId)
//                 .OnDelete(DeleteBehavior.Cascade);

//             // Конфигурация OrderTobacco -> Tobacco
//             modelBuilder
//                 .Entity<OrderTobacco>()
//                 .HasOne(ot => ot.Tobacco)
//                 .WithMany()
//                 .HasForeignKey(ot => ot.TobaccoId)
//                 .OnDelete(DeleteBehavior.Restrict);
//         }
//     }
// }
