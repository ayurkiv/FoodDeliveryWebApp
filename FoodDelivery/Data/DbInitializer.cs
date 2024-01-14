using FoodDelivery.Data;
using FoodDelivery.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

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
            _roleManager.CreateAsync(new IdentityRole("Courier")).GetAwaiter().GetResult();
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
            
            
            
            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "InitialItem.json");
            ImportDataFromJson(jsonFilePath);

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
    
    public void ImportDataFromJson(string jsonFilePath)
    {
        // Зчитати дані з JSON файлу
        string jsonContent = File.ReadAllText(jsonFilePath);
        var foodItemsData = JsonConvert.DeserializeObject<List<FoodItemData>>(jsonContent);

        // Отримати або створити категорію "pizza"
        Category pizzaCategory = _context.Categories.FirstOrDefault(c => c.Title == "pizza");
        if (pizzaCategory == null)
        {
            pizzaCategory = new Category { Title = "pizza" };
            _context.Categories.Add(pizzaCategory);
            _context.SaveChanges();
        }

        // Створити FoodItem та додати їх до категорії "pizza"
        foreach (var foodItemData in foodItemsData)
        {
            FoodItem foodItem = new FoodItem
            {
                Name = foodItemData.Name,
                Description = foodItemData.Description,
                Image = foodItemData.Image,
                Available = foodItemData.Available,
                TimeToReady = foodItemData.TimeToReady,
                Weight = foodItemData.Weight,
                Price = foodItemData.Price,
                CategoryId = pizzaCategory.Id,
            };

            _context.FoodItems.Add(foodItem);
        }

        _context.SaveChanges();
    }
}

// Клас для зберігання даних з JSON
public class FoodItemData
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public bool Available { get; set; }
    public int TimeToReady { get; set; }
    public int Weight { get; set; }
    public float Price { get; set; }
}