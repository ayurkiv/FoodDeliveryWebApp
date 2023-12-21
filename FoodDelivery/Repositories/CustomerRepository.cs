using FoodDelivery.Data;
using FoodDelivery.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Repositories
{
    public class CustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCurrentCustomerIdAsync(string userId)
        {
            var customer = await _context.Customers
                .Where(c => c.ApplicationUserId == userId)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            return customer;
        }

        public async Task<Customer> GetUserWithDetailsAsync(string userId)
        {
            return await _context.Customers
                .Include(c => c.ApplicationUser)
                .Include(c => c.Cart)
                    .ThenInclude(cart => cart.OrderItems)
                        .ThenInclude(orderItem => orderItem.FoodItem)
                .SingleOrDefaultAsync(c => c.ApplicationUserId == userId);
        }
    }
}
