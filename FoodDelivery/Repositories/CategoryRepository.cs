using System.Collections.Generic;
using System.Linq;
using FoodDelivery.Data;
using FoodDelivery.Models;
using FoodDelivery.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Repositories
{
    public class CategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public List<CategoryViewModel> GetPaginatedCategories(int page, int pageSize)
        {
            var categories = _context.Categories
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new CategoryViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    // Map other properties as needed
                })
                .ToList();

            return categories;
        }

        public CategoryViewModel GetCategoryById(int id)
        {
            var category = _context.Categories
                .Where(x => x.Id == id)
                .Select(x => new CategoryViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    // Map other properties as needed
                })
                .FirstOrDefault();

            return category;
        }

        public int CreateCategory(CategoryViewModel vm)
        {
            var category = new Category
            {
                Title = vm.Title,
                // Map other properties as needed
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return category.Id;
        }

        public void UpdateCategory(CategoryViewModel vm)
        {
            var categoryFromDb = _context.Categories.FirstOrDefault(x => x.Id == vm.Id);

            if (categoryFromDb != null)
            {
                categoryFromDb.Title = vm.Title;
                // Update other properties as needed
                _context.SaveChanges();
            }
        }

        public void DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);

            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }

        public int GetTotalItems()
        {
            return _context.Categories.Count(); ;
        }
    }
}
