using FoodDelivery.Data;
using FoodDelivery.Models;
using FoodDelivery.Repositories;
using FoodDelivery.Utilities;
using FoodDelivery.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace FoodDelivery.Controllers
{
    public class HomeController : Controller
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly FoodItemRepository _foodItemRepository;
        private readonly CustomerRepository _customerRepository;

        public HomeController(CategoryRepository categoryRepository, FoodItemRepository foodItemRepository, CustomerRepository customerRepository)
        {
            _categoryRepository = categoryRepository;
            _foodItemRepository = foodItemRepository;
            _customerRepository = customerRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index(string category, int page = 1, int pageSize = 6)
        {
            var categories = _categoryRepository.GetPaginatedCategories(page, pageSize);
            ViewBag.Categories = categories;

            var foodItems = await _foodItemRepository.GetFoodItemsAsync(page, pageSize);

            if (!string.IsNullOrEmpty(category) && category.ToLower() != "all")
            {
                foodItems = foodItems.Where(item => item.CategoryName.ToLower() == category.ToLower()).ToList();
            }

            var paginatedList = new PaginatedList<FoodItemViewModel>(
                foodItems, foodItems.Count, page, pageSize);

            ViewBag.Category = category;

            return View(paginatedList);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = await _customerRepository.GetUserWithDetailsAsync(userId);

            if (customer != null)
            {
                await _foodItemRepository.AddToCart(id, customer);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

    }
}