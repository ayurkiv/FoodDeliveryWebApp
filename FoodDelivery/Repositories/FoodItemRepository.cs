using FoodDelivery.Data;
using FoodDelivery.Models;
using FoodDelivery.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDelivery.Repositories
{

    public class FoodItemRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FoodItemRepository(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<FoodItemViewModel>> GetFoodItemsAsync(int page, int pageSize)
        {
            var items = await _context.FoodItems
                .Where(item => item.IsDelete != true)
                .Include(x => x.Category)
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(item => new FoodItemViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    CategoryId = item.CategoryId,
                    CategoryName = item.Category!.Title,
                    AddedDate = item.AddedDate,
                    Available = item.Available,
                    Weight = item.Weight,
                    TimeToReady = item.TimeToReady,
                    ImageUrl = $"/Images/FoodItems/{item.Image}",
                })
                .ToListAsync();

            return items;
        }


        public async Task<FoodItemViewModel> GetFoodItemAsync(int id)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);

            if (foodItem == null)
            {
                return null;
            }
            else
            {
                var viewModel = new FoodItemViewModel
                {
                    Id = foodItem.Id,
                    Name = foodItem.Name,
                    Description = foodItem.Description,
                    Price = foodItem.Price,
                    CategoryId = foodItem.CategoryId,
                    CategoryName = foodItem?.Category?.Title,
                    Available = foodItem!.Available,
                    AddedDate = foodItem.AddedDate,
                    ImageUrl = foodItem.Image,
                    Weight = foodItem.Weight,
                    TimeToReady = foodItem.TimeToReady,
                };

                return viewModel;

            }
        }

        public async Task<int> CreateFoodItemAsync(FoodItemViewModel viewModel)
        {
            var model = new FoodItem
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Price = viewModel.Price,
                TimeToReady = viewModel.TimeToReady,
                Weight = viewModel.Weight,
                CategoryId = viewModel.CategoryId,
            };

            if (viewModel.Image != null)
            {
                await SaveImageAsync(model, viewModel.Image);
            }

            _context.FoodItems.Add(model);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateFoodItemAsync(FoodItemViewModel viewModel)
        {
            var model = await _context.FoodItems.FindAsync(viewModel.Id);

            if (model == null)
            {
                return 0; // FoodItem not found
            }

            model.Name = viewModel.Name;
            model.Description = viewModel.Description;
            model.Price = viewModel.Price;
            model.CategoryId = viewModel.CategoryId;
            model.Weight = viewModel.Weight;
            model.Available = viewModel.Available;
            model.TimeToReady = viewModel.TimeToReady;
            if (viewModel.Image != null)
            {
                await SaveImageAsync(model, viewModel.Image);
            }
            _context.Entry(model).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteFoodItemAsync(int id)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);

            if (foodItem == null)
            {
                return -1; // FoodItem not found
            }
            _context.FoodItems.Remove(foodItem);
            return await _context.SaveChangesAsync();
        }

        private async Task SaveImageAsync(FoodItem model, IFormFile image)
        {
            string savePath = "/Images/FoodItems/";
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            string imagePath = Path.Combine(wwwRootPath + savePath, fileName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            model.Image = fileName;
        }

        public async Task AddOrderItemToCartAsync(Cart cart, OrderItem orderItem)
        {
            cart?.OrderItems?.Add(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task AddToCart(int FoodItemId, Customer customer)
        {
            var foodItem = await _context.FoodItems.FindAsync(FoodItemId);
            if (foodItem != null)
            {
                var orderItem = new OrderItem
                {
                    FoodItem = foodItem,
                    Amount = 1,
                    OrderItemTotal = foodItem.Price,
                    OrderItemWeight = foodItem.Weight,
                };

                await AddOrderItemToCartAsync(customer.Cart!, orderItem);
            }
        }
        public int GetTotalItems()
        {
            return _context.FoodItems.Count(c => !c.IsDelete);
        }

    }
}