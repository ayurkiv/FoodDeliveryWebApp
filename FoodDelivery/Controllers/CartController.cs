using FoodDelivery.Common;
using FoodDelivery.Data;
using FoodDelivery.Models;
using FoodDelivery.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace FoodDelivery.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CartController : Controller
    {
		private readonly ApplicationDbContext _context;
        
		public CartController(ApplicationDbContext context)
		{
			_context = context;
		}

        [HttpGet]
		public IActionResult Index()
        {
			// Отримати ідентифікатор поточного користувача
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			// Отримати дані корзини з бази даних
			var cartItems = _context.OrderItems
				.Where(item => item.Cart.Customer.ApplicationUserId == userId)
				.Select(item => new OrderItemViewModel
				{
					OrderItemId = item.Id,
                    FoodItemImage = item.FoodItem.Image,
                    FoodItemName = item.FoodItem.Name,
					Amount = item.Amount,
					OrderItemTotal = item.OrderItemTotal
				})
				.ToList();

			return View(cartItems);
        }



		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(int id)
		{
			// Отримати ідентифікатор поточного користувача
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			// Знайти елемент корзини за його ідентифікатором
			var orderItem = _context.OrderItems
				.Include(item => item.Cart.Customer)
				.SingleOrDefault(item => item.Id == id && item.Cart.Customer.ApplicationUserId == userId);

			if (orderItem == null)
			{
				// Обробити ситуацію, коли елемент не знайдено
				return NotFound();
			}

			// Видалити елемент корзини
			_context.OrderItems.Remove(orderItem);
			_context.SaveChanges();

            // Перенаправити користувача на сторінку корзини з оновленими даними
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout()
        {
            // Отримати ідентифікатор поточного користувача
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Отримати користувача та його корзину з бази даних
            var user = _context.Users
                .Include(u => u.Customer)
                .ThenInclude(c => c.Cart)
                .SingleOrDefault(u => u.Id == userId);

            if (user == null || user.Customer == null || user.Customer.Cart == null)
            {
                // Обробити ситуацію, коли користувач або корзина не знайдені
                return NotFound();
            }

            // Отримати елементи корзини для обробки замовлення
            var cartItems = _context.OrderItems
                .Where(item => item.Cart.Customer.ApplicationUserId == userId)
                .ToList();

            if (cartItems.Count == 0)
            {
                // Обробити ситуацію, коли корзина порожня
                return RedirectToAction(nameof(Index));
            }

            // Створити нове замовлення

            // Create a new order with the order address
            var order = new Order
            {
                DeliveryStatus = DeliveryStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                OrderItems = cartItems,
                OrderTotal = cartItems.Sum(item => item.OrderItemTotal),
                WeightTotal = cartItems.Sum(item => item.OrderItemWeight),
                CustomerId = user.Customer?.Id
            };
            // Очистити корзину користувача
            user.Customer.Cart.OrderItems?.Clear();

            // Додати замовлення до бази даних
            _context.Orders.Add(order);

            // Зберегти зміни в базі даних
            _context.SaveChanges();

            // Перенаправити користувача на сторінку підтвердження замовлення
            return RedirectToAction("Confirmation", "Order", new { id = order.Id });
        }
    }
}
