using FoodDelivery.Data;
using FoodDelivery.Models;
using FoodDelivery.Services;
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
        private readonly CategoryService _categoryService;
        private readonly FoodItemService _foodItemService;
        private readonly CustomerService _customerService;

        public HomeController(CategoryService categoryService, FoodItemService foodItemService, CustomerService customerService)
        {
            _categoryService = categoryService;
            _foodItemService = foodItemService;
            _customerService = customerService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index(string category, int page = 1, int pageSize = 6)
        {
            var categories = _categoryService.GetPaginatedCategories(page, pageSize);
            ViewBag.Categories = categories;

            var foodItems = await _foodItemService.GetFoodItemsAsync(page, pageSize);

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
            var user = await _customerService.GetUserWithDetailsAsync(userId);

            if (user != null)
            {
                var foodItem = await _foodItemService.GetFoodItemAsync(id);
                if (foodItem != null)
                {
                    var orderItem = new OrderItem
                    {
                        FoodItem = foodItem,
                        Amount = 1,
                        OrderItemTotal = foodItem.Price,
                        OrderItemWeight = foodItem.Weight,
                    };

                    await _foodItemService.AddOrderItemToCartAsync(user.Cart, orderItem);

                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
