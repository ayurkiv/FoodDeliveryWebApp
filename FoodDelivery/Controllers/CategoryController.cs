using FoodDelivery.Models;
using FoodDelivery.ViewModel;
using Microsoft.AspNetCore.Mvc;
using FoodDelivery.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.Repositories;

namespace FoodDelivery.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly CategoryRepository _categoryService;

        public CategoryController(CategoryRepository categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var categories = _categoryService.GetPaginatedCategories(page, pageSize);

            // Calculate the total number of categories in the database
            int totalItems = _categoryService.GetTotalItems();

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
            int categoryId = _categoryService.CreateCategory(vm);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var viewModel = _categoryService.GetCategoryById(id);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(CategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _categoryService.UpdateCategory(vm);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var viewModel = _categoryService.GetCategoryById(id);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Delete(CategoryViewModel vm)
        {
            _categoryService.DeleteCategory(vm.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
