using FoodDelivery.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FoodDelivery.Repositories;
using FoodDelivery.Utilities;

namespace FoodDelivery.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FoodItemController : Controller
    {
        private readonly FoodItemRepository _foodItemRepository;
        private readonly CategoryRepository _categoryRepository;

        public FoodItemController(FoodItemRepository foodItemService, CategoryRepository categoryRepository)
        {
            _foodItemRepository = foodItemService;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var items = await _foodItemRepository.GetFoodItemsAsync("all", page, pageSize);
            var totalItems = _foodItemRepository.GetTotalItems();

            var paginatedList = new PaginatedList<FoodItemViewModel>(items, totalItems, page, pageSize);

            return View(paginatedList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var categories = _categoryRepository.GetCategories(); // Assuming you have a method in the service to get categories
            ViewBag.Categories = new SelectList(categories, "Id", "Title");

            return View(new FoodItemViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(FoodItemViewModel vm)
        {
            if (ModelState.IsValid)
            {
                await _foodItemRepository.CreateFoodItemAsync(vm);
                return RedirectToAction(nameof(Index));
            }

            var categories = _categoryRepository.GetCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Title");

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _foodItemRepository.GetFoodItemAsync(id);

            if (vm == null)
            {
                return NotFound();
            }

            var categories = _categoryRepository.GetCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Title");

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FoodItemViewModel vm)
        {
            if (ModelState.IsValid)
            {
                await _foodItemRepository.UpdateFoodItemAsync(vm);
                return RedirectToAction(nameof(Index));
            }

            var categories = _categoryRepository.GetCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Title");

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var vm = await _foodItemRepository.GetFoodItemAsync(id);

            if (vm == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _foodItemRepository.DeleteFoodItemAsync(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var vm = await _foodItemRepository.GetFoodItemAsync(id);

            if (vm == null)
            {
                return NotFound();
            }

            return View(vm);
        }
    }
}
