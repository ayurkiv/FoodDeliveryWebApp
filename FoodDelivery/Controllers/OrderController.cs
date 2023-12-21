using FoodDelivery.Data;
using FoodDelivery.Models;
using FoodDelivery.Repositories;
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
    }
}
