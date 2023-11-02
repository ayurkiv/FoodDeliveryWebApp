using FoodDelivery.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Hosting;
using System.Reflection.Emit;
using System;

namespace FoodDelivery.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        //public DbSet<Customer> Customers { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Address)
                .WithMany()
                .HasForeignKey(u => u.AddressId);
            builder.Entity<Address>()
                .HasOne(a => a.User)
                .WithOne(u => u.Address)
                .HasForeignKey<Address>(a => a.UserId);
        


        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());



        }
    }

    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.UserSurname);
            builder.Property(u => u.AddressId);

        }

    }
}