using FoodDelivery.Data;
using FoodDelivery.Models;
using FoodDelivery.Utilities;
using FoodDelivery.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FoodDelivery.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string category, int page = 1, int pageSize = 6)
        {
            // Отримання категорій для панелі сортування
            var categories = _context.Categories.ToList();
            ViewBag.Categories = categories;

            // Запит для отримання продуктів
            var query = _context.FoodItems
                .Include(Item => Item.Category)
                .Select(item => new FoodItemViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    ImageUrl = $"/Images/FoodItems/{item.Image}",
                    CategoryName = item.Category.Title,
                    TimeToReady = item.TimeToReady,
                    Weight = item.Weight,
                    Available = item.Available
                });

            // Фільтрація за категорією
            if (!string.IsNullOrEmpty(category) && category.ToLower() != "all")
            {
                query = query.Where(item => item.CategoryName.ToLower() == category.ToLower());
            }

            // Сортування за категорією
            query = query.OrderBy(item => item.CategoryName);

            // Створення об'єкта PaginatedList для пагінації
            var paginatedList = new PaginatedList<FoodItemViewModel>(
                query.ToList(), query.Count(), page, pageSize);

            ViewBag.Category = category;

            return View(paginatedList);
        }


    }
}