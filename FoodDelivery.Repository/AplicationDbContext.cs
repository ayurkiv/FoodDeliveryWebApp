using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodDelivery.Models;
using Microsoft.AspNetCore.Identity;

namespace FoodDelivery.Repository
{
    public class AplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
        {
        }

    public DbSet<User> Users {  get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}
