using FoodDelivery.Data;
using FoodDelivery.Models;
using Microsoft.AspNetCore.Identity;

public class DbInitializer : IDbInitializer
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public DbInitializer(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
    }

    public void Initialize()
    {
        if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
        {
            // Створюємо роль "Admin", якщо її немає
            _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole("Customer")).GetAwaiter().GetResult();

        }

        string Email = "admin@admin.com";
        string password = "Admin12!";

        var adminUser = _userManager.FindByEmailAsync(Email).GetAwaiter().GetResult();
        if (adminUser == null)
        {
            var Admin = new ApplicationUser
            {
                Email = Email,
                UserName = Email
            };

            _userManager.CreateAsync(Admin, password).GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(Admin, "Admin").GetAwaiter().GetResult();
        }
        else
        {
            // Перевірте, чи користувач вже належить до ролі "Admin"
            var isInRole = _userManager.IsInRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
            if (!isInRole)
            {
                // Додайте користувача до ролі, якщо він не належить до неї
                _userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
            }
        }
    }
}
