using FoodDelivery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Repository
{
    public class DbInitializer : IDbInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AplicationDbContext _context;

        public DbInitializer(RoleManager<IdentityRole> roleManager, 
            UserManager<IdentityUser> userManager,
            AplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public void Initialize()
        {
            try
            {
                if(_context.Database.GetPendingMigrations().Count()>0)
                {
                    _context.Database.Migrate();
                }

            }
            catch(Exception) 
            {
                throw;
            }
            if (_context.Roles.Any(x => x.Name == "Admin")) return;
            _roleManager.CreateAsync(new IdentityRole("Courier")).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole("Customer")).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();

            var admin = new Admin()
            {
                Name = "admin",
                Surname = "admin",
                Phone = "admin",
                Password = "1234",
            };
            _userManager.CreateAsync(admin, "Admin@123").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}
