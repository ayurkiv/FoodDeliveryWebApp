using FoodDelivery.Common;
using FoodDelivery.Data;
using FoodDelivery.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FoodDelivery.Controllers
{
    [Authorize(Roles = "Courier")]
    public class CourierController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourierController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int? GetCurrentCourierId()
        {
            // Отримати ідентифікатор поточного користувача
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Отримати дані користувача з бази даних
            var user = _context.Users
                .Include(u => u.Courier)
                .SingleOrDefault(u => u.Id == userId);

            // Перевірити, чи користувач є кур'єром і повернути ідентифікатор
            return user?.Courier?.Id;
        }

        public IActionResult Index()
        {
            var courierId = GetCurrentCourierId();

            if (courierId == null)
            {
                return NotFound();
            }

            // Отримати список замовлень для кур'єра
            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Address)
                .Include(o => o.Customer)
                .Where(o => o.CourierId == courierId)
                .ToList();

            // Мапування на ViewModel для відображення в представленні
            var orderViewModels = orders.Select(order => new OrderViewModel
            {
                Id = order.Id,
                DeliveryStatus = order.DeliveryStatus,
                PaymentStatus = order.PaymentStatus,
                OrderTotal = order.OrderTotal,
                WeightTotal = order.WeightTotal,
                OrderDate = order.OrderDate,
                OrderItems = order.OrderItems.Select(oi => new OrderItemViewModel
                {
                    OrderItemId = oi.Id,
                    FoodItemName = oi.FoodItem?.Name,
                    Amount = oi.Amount,
                    OrderItemTotal = oi.OrderItemTotal
                }).ToList(),
                AddressId = order.AddressId,
                Address = order.Address,
                CustomerId = order.CustomerId,
                CourierId = order.CourierId
            }).ToList();

            return View(orderViewModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeDeliveryStatus(int id)
        {
            // Отримати ідентифікатор поточного кур'єра
            var courierId = GetCurrentCourierId();

            if (courierId == null)
            {
                // Обробити ситуацію, коли користувач не є кур'єром
                return NotFound();
            }

            // Знайти замовлення за його ідентифікатором та перевірити, чи кур'єр працює над ним
            var order = _context.Orders
                .Include(o => o.Courier)
                .SingleOrDefault(o => o.Id == id && o.Courier.Id == courierId);

            if (order == null)
            {
                // Обробити ситуацію, коли замовлення не знайдено або кур'єр не працює над ним
                return NotFound();
            }

            // Змінити статус доставки
            order.DeliveryStatus = DeliveryStatus.Delivered;

            // Зберегти зміни в базі даних
            _context.SaveChanges();

            // Повернути користувача на сторінку зі списком замовлень
            return RedirectToAction("Index");
        }
    }
}