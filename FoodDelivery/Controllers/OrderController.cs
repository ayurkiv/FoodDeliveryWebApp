using FoodDelivery.Data;
using FoodDelivery.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FoodDelivery.Controllers
{

    [Authorize(Roles = "Customer")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Get the user ID of the current user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve all orders for the current user from the database
            var orders = _context.Orders
                .Where(o => o.Customer.ApplicationUserId == userId)
                .OrderByDescending(o => o.OrderDate) // Order by date, newest first
                .ToList();

            // Create a list of OrderViewModel to display in the view
            var orderViewModels = orders.Select(order => new OrderViewModel
            {
                Id = order.Id,
                DeliveryStatus = order.DeliveryStatus,
                PaymentStatus = order.PaymentStatus,
                OrderTotal = order.OrderTotal,
                WeightTotal = order.WeightTotal,
                OrderDate = order.OrderDate,
                OrderItems = order.OrderItems?.Select(item => new OrderItemViewModel
                {
                    OrderItemId = item.Id,
                    FoodItemName = item.FoodItem.Name,
                    Amount = item.Amount,
                    OrderItemTotal = item.OrderItemTotal
                }).ToList(),
                AddressId = order.AddressId,
                CustomerId = order.CustomerId,
                CourierId = order.CourierId
            }).ToList();

            return View(orderViewModels);
        }


        [HttpGet]
        public IActionResult Confirmation(int id)
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

        [HttpGet]
        public IActionResult Details(int id)
        {
            // Retrieve the order with the specified ID from the database
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FoodItem)
                .Include(o => o.Customer)
                .Include(o => o.Address)
                .SingleOrDefault(o => o.Id == id);

            if (order == null)
            {
                // Handle the case where the order is not found
                return NotFound();
            }

            // Map the Order model to the OrderViewModel (you may need to create a mapping method or library)
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
                AddressId = order.Address?.Id,
                CustomerId = order.Customer?.Id,
                CourierId = order.Courier?.Id
            };

            return View(orderViewModel);
        }

    }
}
