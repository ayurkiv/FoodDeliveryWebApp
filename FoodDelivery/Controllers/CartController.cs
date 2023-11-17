using FoodDelivery.Data;
using FoodDelivery.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FoodDelivery.Controllers
{

    public class CartController : Controller
    {
		private readonly ApplicationDbContext _context;

		public CartController(ApplicationDbContext context)
		{
			_context = context;
		}

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
					FoodItemName = item.FoodItem.Name,
					Amount = item.Amount,
					OrderItemTotal = item.OrderItemTotal
				})
				.ToList();

			return View(cartItems);
        }



		[HttpDelete]
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
			return RedirectToAction(null);
		}

	}
}
