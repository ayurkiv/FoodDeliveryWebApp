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

            return View(listFromDb);
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

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var viewModel =_context.Categories.Where(x => x.Id == id)
                .Select(x => new CategoryViewModel()
                {
                    Id=x.Id,
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
                if(categoryFromDb != null)
                {
                    categoryFromDb.Title = vm.Title;    
                    _context.Categories.Update(categoryFromDb);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Where(x => x.Id == id).FirstOrDefault();
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}
