using FoodDelivery.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Hosting;
using System.Reflection.Emit;
using System;
using Microsoft.AspNetCore.Identity;

namespace FoodDelivery.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Courier> Couriers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }




        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);


            //add AspNetUsers connection with my own database
            builder.Entity<Courier>()
                .HasOne(a => a.ApplicationUser)
                .WithOne(u => u.Courier)
                .HasForeignKey<Courier>(a => a.ApplicationUserId);


            builder.Entity<Customer>()
                .HasOne(a => a.ApplicationUser)
                .WithOne(u => u.Customer)
                .HasForeignKey<Customer>(a => a.ApplicationUserId);


            //add Adress to Customer
            builder.Entity<Address>()
                .HasOne(a => a.Customer)
                .WithOne(u => u.Address)
                .HasForeignKey<Address>(a => a.CustomerId);

            //Order to orderitem
            builder.Entity<OrderItem>()
                .HasOne(a => a.Order)
                .WithMany(Order => Order.OrderItems)
                .HasForeignKey(a => a.OrderId);

            builder.Entity<Order>()
                .HasMany(a => a.OrderItems);

            //Customer order
            builder.Entity<Customer>()
                .HasMany(a => a.Orders);

            builder.Entity<Order>()
                .HasOne(a => a.Customer)
                .WithMany(Customer => Customer.Orders)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(deleteBehavior: DeleteBehavior.NoAction);


            //Ordere courier
            builder.Entity<Courier>()
                .HasMany(a => a.Orders);

            builder.Entity<Order>()
                .HasOne(a => a.Courier)
                .WithMany(Customer => Customer.Orders)
                .HasForeignKey(a => a.CourierId)
                .OnDelete(deleteBehavior: DeleteBehavior.NoAction);


            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        }
    }

    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.LastName);
            builder.Property(u => u.FirstName);
            builder.Property(u => u.CourierId);
            builder.Property(u => u.CustomerId);


        }

    }
}