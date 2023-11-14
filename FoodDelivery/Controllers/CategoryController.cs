using FoodDelivery.Data;
using FoodDelivery.Models;
using FoodDelivery.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers
{
    
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var categories = _context.Categories
                .OrderBy(x => x.Id) // Order by a suitable property
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new CategoryViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    // Map other properties as needed
                })
                .ToList();

            // Calculate the total number of categories in the database
            int totalItems = _context.Categories.Count();

            // Create a PaginatedList to pass to the view
            var paginatedList = new PaginatedList<CategoryViewModel>(categories, totalItems, page, pageSize);

            return View(paginatedList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            CategoryViewModel newCategory = new CategoryViewModel();

            return View(newCategory);
        }

        [HttpPost]
        public IActionResult Create(CategoryViewModel vm)
        {
            Category model = new Category();
            model.Title = vm.Title;
            _context.Categories.Add(model);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var viewModel = _context.Categories.Where(x => x.Id == id)
                .Select(x => new CategoryViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                }).FirstOrDefault();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(CategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var categoryFromDb = _context.Categories.FirstOrDefault(x => x.Id == vm.Id);
                if (categoryFromDb != null)
                {
                    categoryFromDb.Title = vm.Title;
                    _context.Categories.Update(categoryFromDb);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var viewModel = _context.Categories.Where(x => x.Id == id)
                .Select(x => new CategoryViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                })
                .FirstOrDefault();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Delete(CategoryViewModel vm)
        {
            var category = _context.Categories.Where(x => x.Id == vm.Id).FirstOrDefault();
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
