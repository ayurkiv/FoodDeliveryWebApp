using System.Linq;
using FoodDelivery.Data;
using FoodDelivery.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Repositories
{
    public class OrderItemRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public OrderItem GetOrderItemById(int id)
        {
            return _context.OrderItems
                .Where(oi => oi.Id == id)
                .Include(oi => oi.FoodItem)
                .Include(oi => oi.Order)
                .Include(oi => oi.Cart)
                .SingleOrDefault();
        }
    }
}
