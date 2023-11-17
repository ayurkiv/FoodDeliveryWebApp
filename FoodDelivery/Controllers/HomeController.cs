using FoodDelivery.Data;
using FoodDelivery.Models;
using FoodDelivery.Utilities;
using FoodDelivery.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace FoodDelivery.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

		[HttpGet]
        public IActionResult Index(string category, int page = 1, int pageSize = 6)
        {
            // Отримання категорій для панелі сортування
            var categories = _context.Categories.ToList();
            ViewBag.Categories = categories;

            // Запит для отримання продуктів
            var query = _context.FoodItems
                .Include(Item => Item.Category)
                .Select(item => new FoodItemViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    ImageUrl = $"/Images/FoodItems/{item.Image}",
                    CategoryName = item.Category.Title,
                    TimeToReady = item.TimeToReady,
                    Weight = item.Weight,
                    Available = item.Available
                });

            // Фільтрація за категорією
            if (!string.IsNullOrEmpty(category) && category.ToLower() != "all")
            {
                query = query.Where(item => item.CategoryName.ToLower() == category.ToLower());
            }

            // Сортування за категорією
            query = query.OrderBy(item => item.CategoryName);

            // Створення об'єкта PaginatedList для пагінації
            var paginatedList = new PaginatedList<FoodItemViewModel>(
                query.ToList(), query.Count(), page, pageSize);

            ViewBag.Category = category;

            return View(paginatedList);
        }

		[Authorize(Roles = "Customer")]
		[HttpPost]
		public IActionResult AddToCart(int id)
		{
			Console.WriteLine("AddToCart"+ id);
		//	// Отримати ідентифікатор користувача
		//	var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

		//	// Отримати користувача з бази даних
		//	var user = _context.Users
		//		.Include(u => u.Customer)
		//		.SingleOrDefault(u => u.Id == userId);

		//	if (user == null)
		//	{
		//		// Якщо користувач не знайдений, переадресувати на сторінку реєстрації або іншу сторінку за власним вибором
		//		return RedirectToAction("Register", "Account");
		//	}

		//	// Отримати товар з бази даних
		//	var foodItem = _context.FoodItems.SingleOrDefault(item => item.Id == id);

		//	if (foodItem != null)
		//	{
		//		// Додати товар до корзини користувача
		//		var orderItem = new OrderItem
		//		{

		//			FoodItem = foodItem,
		//			Amount = 1, // Кількість можна вказати відповідно до вашого дизайну
		//			OrderItemTotal = foodItem.Price
		//		};
		//		_context.OrderItems.Add(orderItem);

		//		if (user.Customer.Cart == null)
		//		{
		//			user.Customer.Cart = new Cart();
		//		}

		//		user.Customer.Cart.OrderItems.Add(orderItem);

		//		// Зберегти зміни в базі даних
		//		_context.SaveChanges();

		//		// Можна використовувати TempData або інші методи для передачі повідомлень про успішну операцію на клієнтський бік
		//		TempData["SuccessMessage"] = "Item added to the cart successfully.";
		//	}
		//	else
		//	{
		//		// Можна використовувати TempData або інші методи для передачі повідомлень про помилку на клієнтський бік
		//		TempData["ErrorMessage"] = "Item not found.";
		//	}

		//	// Повернення на попередню сторінку або іншу сторінку за власним вибором
			return RedirectToAction(null);
		}
	}

}
