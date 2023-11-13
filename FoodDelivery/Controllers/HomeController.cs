using FoodDelivery.Data;
using FoodDelivery.Models;
using FoodDelivery.Utilities;
using FoodDelivery.ViewModel;
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

        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var items = _context.FoodItems.Include(x => x.Category)
                .OrderBy(x => x.AddedDate)  // Додайте цей рядок для сортування
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModels = items.Select(item => new FoodItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Amount = item.Amount,
                Price = item.Price,
                AddedDate = item.AddedDate,
                CategoryId = item.CategoryId,
                CategoryName = item.Category?.Title,
                ImageUrl = $"/Images/FoodItems/{item.Image}"
                // Додайте інші властивості за необхідності
            }).ToList();

            var totalItems = _context.FoodItems.Count();
            var paginatedList = new PaginatedList<FoodItemViewModel>(viewModels, totalItems, page, pageSize);

            return View(paginatedList);
        }
    }
}