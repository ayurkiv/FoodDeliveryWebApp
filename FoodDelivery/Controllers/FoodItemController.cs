using FoodDelivery.Data;
using FoodDelivery.Models;
using FoodDelivery.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;


namespace FoodDelivery.Controllers
{
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
        public IActionResult Index()
        {
            var items = _context.FoodItems.Include(x => x.Category).ToList();
            var viewModels = items.Select(item => new FoodItemViewModel
            {
                // Map properties from FoodItem to FoodItemViewModel
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Amount = item.Amount,
                Price = item.Price,
                AddedDate = item.AddedDate,
                MenuId = item.MenuId,
                CategoryTitle = item.Category.Title,
                CategoryId = item.CategoryId,
                // Map other properties as needed
            }).ToList();

            return View(viewModels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            FoodItemViewModel vm = new FoodItemViewModel();
            ViewBag.Category = new SelectList(_context.Categories, "Id", "Title");
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FoodItemViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if(vm.Image != null)
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
                        MenuId = vm.MenuId,
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
            var item = _context.FoodItems.Where(x => x.Id == id).FirstOrDefault();

            // Create a FoodItemViewModel and map properties from FoodItem
            var viewModel = new FoodItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                CategoryId = item.CategoryId,
                // Map other properties as needed
            };

            ViewBag.Category = new SelectList(_context.Categories, "Id", "Title", viewModel.CategoryId);

            // Pass the FoodItemViewModel to the view
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(FoodItemViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    
                    // Retrieve the existing item from the database
                    var existingItem = await _context.FoodItems.FindAsync(model.Id);

                    // Update the properties of the existing item with the new values
                    existingItem.Name = model.Name;
                    existingItem.CategoryId = model.CategoryId;
                    // Update other properties as needed

                    // Check if a new image is provided
                    if (model.Image != null)
                    {
                        // Save the new image file to the wwwroot/images folder
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                        string imagePath = Path.Combine(wwwRootPath, "images", fileName);

                        using (var fileStream = new FileStream(imagePath, FileMode.Create))
                        {
                            await model.Image.CopyToAsync(fileStream);
                        }

                        // Set the Image property in the existing item
                        existingItem.Image = fileName;
                    }

                    // Update the item in the database
                    _context.Update(existingItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            // If ModelState is not valid, reload the page with the existing model
            ViewBag.Category = new SelectList(_context.Categories, "Id", "Title", model.CategoryId);
            return View(model);
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