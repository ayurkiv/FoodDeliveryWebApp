using System.Linq;
using System.Threading.Tasks;
using FoodDelivery.Data;
using FoodDelivery.Models;
using FoodDelivery.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Repositories
{
    public class CartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<bool> DeleteOrderItemFromCartAsync(int orderItemId)
        {
            var orderItem = await _context.OrderItems.FindAsync(orderItemId);

            if (orderItem == null)
            {
                return false; // Item not found
            }

            // Set CartId to null to disassociate the item from the cart
            orderItem.CartId = null;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCurrentCartIdAsync(int customerId)
        {
            var cartId = await _context.Carts
                .Where(cart => cart.CustomerId == customerId)
                .Select(cart => cart.Id)
                .FirstOrDefaultAsync();

            if (cartId == 0)
            {
                // If a cart doesn't exist for the customer, create a new cart
                var newCart = new Cart { CustomerId = customerId };
                _context.Carts.Add(newCart);
                await _context.SaveChangesAsync();
                cartId = newCart.Id;
            }

            return cartId;
        }

        public async Task<bool> AddCartItemAsync(int foodItemId, int customerId, int amount)
        {
            var cartId = await GetCurrentCartIdAsync(customerId);

            var cartItem = await _context.OrderItems
                .Where(item => item.CartId == cartId && item.FoodItemId == foodItemId)
                .FirstOrDefaultAsync();

            if (cartItem == null)
            {
                // If the item doesn't exist in the cart, add a new cart item
                var newCartItem = new OrderItem
                {
                    FoodItemId = foodItemId,
                    CartId = cartId,
                    Amount = amount,
                    // Other properties initialization
                };

                _context.OrderItems.Add(newCartItem);
            }
            else
            {
                // If the item already exists in the cart, update the quantity
                cartItem.Amount += amount;
                // Update any other relevant properties
                _context.OrderItems.Update(cartItem);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveCartItemAsync(int orderItemId, int customerId)
        {
            var cartId = await GetCurrentCartIdAsync(customerId);

            var cartItem = await _context.OrderItems
                .Where(item => item.CartId == cartId && item.Id == orderItemId)
                .FirstOrDefaultAsync();

            if (cartItem == null)
            {
                return false; // Item not found in the cart
            }

            _context.OrderItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCartAsync(Cart cart)
        {
            if (cart == null)
            {
                return false;
            }

            var cartItems = await _context.OrderItems
                .Where(item => item.CartId == cart.Id)
                .ToListAsync();

            foreach (var cartItem in cartItems)
            {
                await DeleteOrderItemFromCartAsync(cartItem.Id);
            }

            return true;
        }

        public async Task<List<OrderItemViewModel>> GetCartItemsViewModelForCustomerAsync(int customerId)
        {
            var cartId = await GetCurrentCartIdAsync(customerId);

            var cartItems = await _context.OrderItems
                .Where(item => item.CartId == cartId)
                .Select(item => new OrderItemViewModel
                {
                    OrderItemId = item.Id,
                    FoodItemImage = item.FoodItem.Image,
                    FoodItemName = item.FoodItem.Name,
                    Amount = item.Amount,
                    OrderItemTotal = item.OrderItemTotal
                })
                .ToListAsync();

            return cartItems;
        }

        public async Task<List<OrderItem>> GetCartItemsForCustomerAsync(int customerId)
        {
            var cartId = await GetCurrentCartIdAsync(customerId);

            var cartItems = await _context.OrderItems
                .Where(item => item.CartId == cartId)
                .ToListAsync();

            return cartItems;
        }

    }
}
