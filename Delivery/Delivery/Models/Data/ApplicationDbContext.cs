using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Net;

namespace Delivery.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Налаштування зв'язків між моделями

            // Зв'язок "один до одного" між User та Address
            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId);

            // Зв'язок "один до багатьох" між User та Order
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Зв'язок "один до багатьох" між Admin та FoodItem
            modelBuilder.Entity<Admin>()
                .HasMany(a => a.FoodItems)
                .WithOne(f => f.AddedBy)
                .HasForeignKey(f => f.AddedByAdminId);

            // Зв'язок "багато до багатьох" між FoodItem та Menu
            modelBuilder.Entity<Menu>()
                .HasKey(m => new { m.FoodItemId, m.Id });

            modelBuilder.Entity<Menu>()
                .HasOne(m => m.FoodItem)
                .WithMany(f => f.Menus)
                .HasForeignKey(m => m.FoodItemId);


            // Зв'язок "один до багатьох" між Order та OrderItem
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Зв'язок "один до багатьох" між Order та Payment
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}