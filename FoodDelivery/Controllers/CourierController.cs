using FoodDelivery.Data;
using FoodDelivery.Models;
using FoodDelivery.Models.Common;
using FoodDelivery.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
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

        private void ChangeCourierStatus(CourierStatus courierStatus)
        {
            var courier = GetCourierById(GetCurrentCourierId());
            courier.CourierStatus = courierStatus;
            _context.SaveChanges();
        }

        private Courier GetCourierById(int courierId) => _context.Couriers
                .FirstOrDefault(c => c.Id == courierId);

        private int GetCurrentCourierId()
        {
            // Отримати ідентифікатор поточного користувача
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Отримати дані користувача з бази даних
            var user = _context.Users
                .Include(u => u.Courier)
                .SingleOrDefault(u => u.Id == userId);

            // Перевірити, чи користувач є кур'єром і повернути ідентифікатор
            return user.Courier.Id;
        }

        public IActionResult Index()
        {
            var courierId = GetCurrentCourierId();

            if (courierId == null)
            {
                return NotFound();
            }

            Courier courier = GetCourierById(courierId);
            ViewBag.CourierStatus = courier?.CourierStatus ?? CourierStatus.Free;

            // Отримати список замовлень для кур'єра та відсортувати їх за статусом доставки
            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.Address)
                .Include(o => o.Customer)
                .Where(o => o.CourierId == courierId)
                .OrderBy(o => o.DeliveryStatus) // Сортування за статусом доставки (по зростанню)
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
                OrderItems = order?.OrderItems?.Select(oi => new OrderItemViewModel
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
        public IActionResult DeliveredOrder(int id)
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
                .SingleOrDefault(o => o.Id == id && o.Courier!.Id == courierId);

            if (order == null)
            {
                // Обробити ситуацію, коли замовлення не знайдено або кур'єр не працює над ним
                return NotFound();
            }

            // Змінити статус доставки
            order.DeliveryStatus = DeliveryStatus.Delivered;

            // Перевірити, чи у кур'єра немає виданих замовлень
            var issuedOrdersCount = _context.Orders.Count(o => o.CourierId == courierId && o.DeliveryStatus == DeliveryStatus.Delivered);

            if (issuedOrdersCount == 0)
            {
                // Якщо у кур'єра немає виданих замовлень, встановити статус "Free"
                ChangeCourierStatus(CourierStatus.Free);
            }

            // Зберегти зміни в базі даних
            _context.SaveChanges();

            // Повернути користувача на сторінку зі списком замовлень
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeclineOrder(int id)
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
                .SingleOrDefault(o => o.Id == id && o.Courier!.Id == courierId);

            if (order == null)
            {
                // Обробити ситуацію, коли замовлення не знайдено або кур'єр не працює над ним
                return NotFound();
            }

            // Змінити статус доставки на "очікування курєра"
            order.DeliveryStatus = DeliveryStatus.Pending;
            order.Courier = null;
            order.CourierId = null;

            ChangeCourierStatus(CourierStatus.Free);

            // Зберегти зміни в базі даних
            _context.SaveChanges();

            // Повернути користувача на сторінку зі списком замовлень
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeCourierStatus(string newCourierStatus)
        {
            var courierId = GetCurrentCourierId();
            var courier = GetCourierById(courierId);

            if (courier != null)
            {
                // Перевірка, чи кур'єр може змінити статус на "breakup"
                if (newCourierStatus == CourierStatus.OnBreak.ToString() && courier.Orders?.Count > 0)
                {
                    // Кур'єр не може бути на перерві, якщо у нього є замовлення
                    return BadRequest("Courier has orders and cannot be on break.");
                }

                // Перевірка, чи кур'єр може приймати нові замовлення
                if (newCourierStatus == CourierStatus.Free.ToString() && courier.Orders?.Count >= 2)
                {
                    // Кур'єр не може приймати більше двох замовлень
                    return BadRequest("Courier cannot accept more than two orders.");
                }

                courier.CourierStatus = Enum.Parse<CourierStatus>(newCourierStatus);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}