using FoodDelivery.Data;
using FoodDelivery.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            // Отримати замовлення за його ідентифікатором
            var order = _context?.Orders?
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FoodItem)
                .SingleOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            // Мапуємо дані замовлення на модель представлення
            var orderViewModel = new OrderViewModel
            {
                Id = order.Id,
                DeliveryStatus = order.DeliveryStatus,
                PaymentStatus = order.PaymentStatus,
                OrderTotal = order.OrderTotal,
                WeightTotal = order.WeightTotal,
                OrderDate = order.OrderDate,
                OrderItems = order.OrderItems?.Select(oi => new OrderItemViewModel
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
            };

            return View(orderViewModel);
        }
    }
}
