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

                // Map ViewModel to Model
                FoodItem model = new FoodItem
                {
                    // Assuming properties in FoodItemViewModel are the same as in FoodItem
                    Name = vm.Name,
                    // Map other properties accordingly
                    CategoryId = vm.CategoryId,
                    // Additional mappings as needed
                };

                // Save the image file to the wwwroot/images folder
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + "_" + vm.Image.FileName;
                string imagePath = Path.Combine(wwwRootPath + "/Images/FoodItems/", fileName);
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

            // If ModelState is not valid, reload the page with the existing model
            ViewBag.Category = new SelectList(_context.Categories, "Id", "Title", vm.CategoryId);
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