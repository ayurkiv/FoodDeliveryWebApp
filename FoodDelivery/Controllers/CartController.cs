using FoodDelivery.Data;
using FoodDelivery.Models;
using FoodDelivery.Models.Common;
using FoodDelivery.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FoodDelivery.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CustomerRepository _customerRepository;
        private readonly OrderItemRepository _orderItemRepository;
        private readonly OrderRepository _orderRepository;
        private readonly CartRepository _cartRepository;

        public CartController(
            ApplicationDbContext context,
            CustomerRepository customerRepository,
            OrderItemRepository orderItemRepository,
            OrderRepository orderRepository,
            CartRepository cartRepository)
        {
            _context = context;
            _customerRepository = customerRepository;
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = await _customerRepository.GetUserWithDetailsAsync(userId);

            if (customer == null)
            {
                return NotFound();
            }

            var cartItems = await _cartRepository.GetCartItemsViewModelForCustomerAsync(customer.Id);

            return View(cartItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int foodItemId, int amount)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = await _customerRepository.GetUserWithDetailsAsync(userId);

            if (customer == null)
            {
                return NotFound();
            }

            await _cartRepository.AddCartItemAsync(foodItemId, customer.Id, amount);

            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int orderItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = await _customerRepository.GetUserWithDetailsAsync(userId);

            if (customer == null)
            {
                return NotFound();
            }

            var success = await _cartRepository.RemoveCartItemAsync(orderItemId, customer.Id);

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = await _customerRepository.GetUserWithDetailsAsync(userId);

            if (customer == null || customer.Cart == null)
            {   
                return NotFound();
            }

            var cartItems = await _cartRepository.GetCartItemsForCustomerAsync(customer.Id);

            if (cartItems.Count == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var order = new Order
                {
                    DeliveryStatus = DeliveryStatus.Pending,
                    PaymentStatus = PaymentStatus.Pending,
                    OrderItems = cartItems,
                    OrderTotal = cartItems.Sum(item => item.OrderItemTotal),
                    WeightTotal = cartItems.Sum(item => item.OrderItemWeight),
                    CustomerId = customer?.Id
                };

                await _orderRepository.AddOrderAsync(order);
                await _orderRepository.AddOrderItemsToOrderAsync(order.Id, cartItems);
                await _cartRepository.ClearCartAsync(customer?.Cart!);

                return RedirectToAction("Confirmation", "Order", new { id = order.Id });

            }
        }
    }
}
