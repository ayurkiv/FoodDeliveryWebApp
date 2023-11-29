using FoodDelivery.Models;
using FoodDelivery.ViewModel;
using FoodDelivery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FoodDelivery.Utilities;

namespace FoodDelivery.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FoodItemController : Controller
    {
        private readonly FoodItemService _foodItemService;

        public FoodItemController(FoodItemService foodItemService)
        {
            _foodItemService = foodItemService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var items = await _foodItemService.GetFoodItemsAsync(page, pageSize);

            var paginatedList = new PaginatedList<FoodItemViewModel>(items, items.Count, page, pageSize);

            return View(paginatedList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var categories = _foodItemService.GetCategories(); // Assuming you have a method in the service to get categories
            ViewBag.Categories = new SelectList(categories, "Id", "Title");

            return View(new FoodItemViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(FoodItemViewModel vm)
        {
            if (ModelState.IsValid)
            {
                await _foodItemService.CreateFoodItemAsync(vm);
                return RedirectToAction(nameof(Index));
            }

            var categories = _foodItemService.GetCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Title");

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _foodItemService.GetFoodItemAsync(id);

            if (vm == null)
            {
                return NotFound();
            }

            var categories = _foodItemService.GetCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Title");

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FoodItemViewModel vm)
        {
            if (ModelState.IsValid)
            {
                await _foodItemService.UpdateFoodItemAsync(vm);
                return RedirectToAction(nameof(Index));
            }

            var categories = _foodItemService.GetCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Title");

            return View(vm);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var vm = _foodItemService.GetFoodItemAsync(id);

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
            await _foodItemService.DeleteFoodItemAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var vm = await _foodItemService.GetFoodItemAsync(id);

            if (vm == null)
            {
                return NotFound();
            }

            return View(vm);
        }
    }
}
