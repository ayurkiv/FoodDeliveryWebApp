using FoodDelivery.Data;
using FoodDelivery.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var listFromDb = _context.Categories
                .ToList().Select( x=> new CategoryViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,

                }).ToList();

            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            CategoryViewModel newCategory = new CategoryViewModel();  

            return View(newCategory);
        }

    }
}
