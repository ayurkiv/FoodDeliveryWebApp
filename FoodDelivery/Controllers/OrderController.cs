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
            // Виконайте обробку платежу тут
            // Перевірте і збережіть статус оплати у замовленні
            var order = _orderRepository.GetOrderById(paymentViewModel.OrderId);
            if (order == null)
            {
                return NotFound();
            }

            // Логіка обробки платежу тут

            // Позначте замовлення як оплачене
            order.PaymentStatus = PaymentStatus.Completed;
            _orderRepository.UpdateOrderPaymentStatus(order);

            // Attempt to assign a courier to the order
            var assignmentSuccess = _orderRepository.AssignCourierToOrderAsync(order.Id).Result; // Synchronously waiting for the result

            if (!assignmentSuccess)
            {
                // Handle the case when no courier is available
                // You might want to display a message or take other actions
                return RedirectToAction("NoCourierAvailable");
            }

            // Редирект на сторінку відстеження замовлення
            return RedirectToAction("Details", new { id = order.Id });
        }


    }
}
