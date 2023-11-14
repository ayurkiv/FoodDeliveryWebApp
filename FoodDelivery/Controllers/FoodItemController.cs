using FoodDelivery.Data;
using FoodDelivery.Models;
using FoodDelivery.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using FoodDelivery.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace FoodDelivery.Controllers
{

    [Authorize(Roles = "Admin")]
    public class FoodItemController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _webHostEnvironment;

        public FoodItemController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            // Retrieve all FoodItems from the database
            var items = _context.FoodItems.Include(x => x.Category).OrderBy(x => x.Id).ToList();

            // Create a PaginatedList to pass to the view
            var paginatedList = new PaginatedList<FoodItemViewModel>(items.Select(item => new FoodItemViewModel
            {
                // Map properties from FoodItem to FoodItemViewModel
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Amount = item.Amount,
                Price = item.Price,
                AddedDate = item.AddedDate,
                CategoryId = item.CategoryId,
                CategoryName = item.Category.Title
                // Map other properties as needed
            }).ToList(), items.Count, page, pageSize);

            return View(paginatedList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            FoodItemViewModel vm = new FoodItemViewModel();
            // Отримайте список категорій з бази даних або іншого джерела
            var categories = _context.Categories.ToList();

            // Передайте список категорій в ViewBag
            ViewBag.Categories = new SelectList(categories, "Id", "Title");
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FoodItemViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.Image != null)
                {
                    // Map ViewModel to Model
                    FoodItem model = new FoodItem
                    {
                        // Assuming properties in FoodItemViewModel are the same as in FoodItem
                        Name = vm.Name,
                        Description = vm.Description,
                        Amount = vm.Amount,
                        Price = vm.Price,
                        AddedDate = DateTime.Now,
                        // Map other properties accordingly
                        CategoryId = vm.CategoryId,
                        // Additional mappings as needed
                    };

                    // Save the image file to the wwwroot/images folder
                    string savePath = "/Images/FoodItems/";
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + vm.Image.FileName;
                    string imagePath = Path.Combine(wwwRootPath + savePath, fileName);
                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        await vm.Image.CopyToAsync(fileStream);
                    }

                    // Set the Image property in the FoodItem model
                    model.Image = fileName;

                    // Add the new FoodItem to the database
                    _context.FoodItems.Add(model);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Add a validation error if no image is provided
                    ModelState.AddModelError("Image", "Please select an image.");
                    ViewBag.Category = new SelectList(_context.Categories, "Id", "Title", vm.CategoryId);
                    return View(vm);
                }
            }

            // If ModelState is not valid, reload the page with the existing model
            ViewBag.Category = new SelectList(_context.Categories, "Id", "Title", vm.CategoryId);
            return View(vm);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Отримайте FoodItem за його ідентифікатором з бази даних
            FoodItem foodItem = _context.FoodItems.Find(id);

            if (foodItem == null)
            {
                // Обробка випадку, коли не знайдено FoodItem за вказаним ідентифікатором
                return NotFound();
            }

            // Отримайте список категорій з бази даних або іншого джерела
            var categories = _context.Categories.ToList();
            FoodItemViewModel vm = new FoodItemViewModel
            {
                Id = foodItem.Id,
                Name = foodItem.Name,
                Description = foodItem.Description,
                Amount = foodItem.Amount,
                Price = foodItem.Price,
                CategoryId = foodItem.CategoryId,
                // Заповніть інші властивості відповідно
            };

            // Передайте список категорій та FoodItemViewModel в ViewBag
            ViewBag.Categories = new SelectList(categories, "Id", "Title");
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FoodItemViewModel vm)
        {
            if (ModelState.IsValid)
            {
                // Отримайте FoodItem за його ідентифікатором з бази даних
                FoodItem model = _context.FoodItems.Find(vm.Id);

                if (model == null)
                {
                    // Обробка випадку, коли не знайдено FoodItem за вказаним ідентифікатором
                    return NotFound();
                }

                // Оновіть властивості FoodItem на основі властивостей FoodItemViewModel
                model.Name = vm.Name;
                model.Description = vm.Description;
                model.Amount = vm.Amount;
                model.Price = vm.Price;
                model.CategoryId = vm.CategoryId;

                if (vm.Image != null)
                {
                    // Оновіть зображення, якщо воно надано
                    string savePath = "/Images/FoodItems/";
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + vm.Image.FileName;
                    string imagePath = Path.Combine(wwwRootPath + savePath, fileName);

                    // Збережіть нове зображення на сервері
                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        await vm.Image.CopyToAsync(fileStream);
                    }

                    // Оновіть властивість Image у FoodItem
                    model.Image = fileName;
                }

                // Оновіть запис в базі даних
                _context.Entry(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var modelStateKey in ViewData.ModelState.Keys)
                {
                    var modelStateVal = ViewData.ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        Console.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }
                }

            }
            // Якщо ModelState не є дійсним, поверніть сторінку редагування з помилками валідації
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Title", vm.CategoryId);
            return View(vm);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Отримайте FoodItem за його ідентифікатором з бази даних
            FoodItem foodItem = _context.FoodItems.Find(id);

            if (foodItem == null)
            {
                // Обробка випадку, коли не знайдено FoodItem за вказаним ідентифікатором
                return NotFound();
            }

            // Create a FoodItemViewModel and populate it with data from the FoodItem
            FoodItemViewModel vm = new FoodItemViewModel
            {
                Id = foodItem.Id,
                Name = foodItem.Name,
                Description = foodItem.Description,
                Amount = foodItem.Amount,
                Price = foodItem.Price,
                AddedDate = foodItem.AddedDate,
                CategoryId = foodItem.CategoryId,
                // Populate other properties as needed
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Отримайте FoodItem за його ідентифікатором з бази даних
            FoodItem foodItem = _context.FoodItems.Find(id);

            if (foodItem == null)
            {
                // Обробка випадку, коли не знайдено FoodItem за вказаним ідентифікатором
                return NotFound();
            }

            // Видаліть FoodItem з бази даних
            _context.FoodItems.Remove(foodItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            // Retrieve the FoodItem from the database based on the provided id
            var foodItem = _context.FoodItems.Find(id);

            if (foodItem == null)
            {
                // Handle the case where the FoodItem with the given id is not found
                return NotFound();
            }

            // Map the FoodItem entity to your FoodItemViewModel if needed
            FoodItemViewModel vm = new FoodItemViewModel
            {
                Id = foodItem.Id,
                Name = foodItem.Name,
                Description = foodItem.Description,
                Amount = foodItem.Amount,
                Price = foodItem.Price,
                AddedDate = foodItem.AddedDate,
                CategoryName = foodItem.Category?.Title,
                ImageUrl = foodItem.Image
            };

            return View(vm);
        }

    }
}

/*if (vm.ImageUrl != null && vm.ImageUrl.Length > 0)
{
    var uploadDir = @"Images/FoodItems";
    var filename = Guid.NewGuid().ToString() + "-" + vm.ImageUrl.FileName();
    var path = Path.Combine(_webHostEnvironment.WebRootPath, uploadDir, filename);
    await vm.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
    model.Image = "/" + uploadDir + "/" + filename;
}*/