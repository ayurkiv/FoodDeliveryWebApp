using FoodDelivery.Data;
using FoodDelivery.Models;
using FoodDelivery.Models.Common;
using FoodDelivery.Repositories;
using FoodDelivery.ViewModel;
using FoodDelivery.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FoodDelivery.Controllers
{

    [Authorize(Roles = "Customer")]
    public class OrderController : Controller
    {
        private readonly OrderRepository _orderRepository;

        public OrderController(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orderViewModels = _orderRepository.GetOrdersForUser(userId!);
            return View(orderViewModels);
        }

        [HttpGet]
        public IActionResult Confirmation(int id)
        {
            var orderViewModel = _orderRepository.GetOrderById(id);

            if (orderViewModel == null)
            {
                return NotFound();
            }

            return View(orderViewModel);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var orderViewModel = _orderRepository.GetOrderById(id);

            if (orderViewModel == null)
            {
                return NotFound();
            }

            return View(orderViewModel);
        }

        [HttpGet]
        public IActionResult Pay(int id)
        {
            var orderViewModel = _orderRepository.GetOrderById(id);

            if (orderViewModel == null)
            {
                return NotFound();
            }

            // Перевірте, чи замовлення ще не оплачено, щоб уникнути подвійної оплати
            if (orderViewModel.PaymentStatus == PaymentStatus.Completed)
            {
                // Редирект на сторінку відстеження замовлення
                return RedirectToAction("Details", new { id = orderViewModel.Id });
            }

            // Передайте дані замовлення до view
            var paymentViewModel = new PaymentViewModel
            {
                OrderId = orderViewModel.Id,
                // Додайте інші необхідні дані для обробки платежу
            };

            return View(paymentViewModel);
        }

        [HttpPost]
        public IActionResult Pay(PaymentViewModel paymentViewModel)
        {
            var order = _orderRepository.GetOrderById(paymentViewModel.OrderId);
            if (order == null)
            {
                return NotFound();
            }

            // Process payment logic here

            order.PaymentStatus = PaymentStatus.Completed;
            _orderRepository.UpdateOrderPaymentStatus(order);

            // Return an immediate response to the client
            return RedirectToAction("Details", new { id = order.Id });
        }

    }
}
