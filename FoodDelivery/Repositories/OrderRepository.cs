using System;
using System.Collections.Generic;
using System.Linq;
using FoodDelivery.Common;
using FoodDelivery.Data;
using FoodDelivery.Models;
using FoodDelivery.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Repositories
{
    public class OrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<OrderViewModel> GetOrdersForUser(string userId)
        {
            var orders = _context?.Orders?
                .Where(o => o.Customer!.ApplicationUserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .Include(o => o.OrderItems)!
                .ThenInclude(oi => oi.FoodItem)
                .ToList();

            var orderViewModels = orders?.Select(order => new OrderViewModel
            {
                Id = order.Id,
                DeliveryStatus = order.DeliveryStatus,
                PaymentStatus = order.PaymentStatus,
                OrderTotal = order.OrderTotal,
                WeightTotal = order.WeightTotal,
                OrderDate = order.OrderDate,
                OrderItems = order.OrderItems?.Select(item => new OrderItemViewModel
                {
                    OrderItemId = item.Id,
                    FoodItemName = item?.FoodItem?.Name,
                    Amount = item!.Amount,
                    OrderItemTotal = item.OrderItemTotal
                }).ToList(),
                AddressId = order.AddressId,
                CustomerId = order.CustomerId,
                CourierId = order.CourierId
            }).ToList();
            return orderViewModels;
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public OrderViewModel GetOrderById(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FoodItem)
                .Include(o => o.Address)
                .Include(o => o.Customer)
                .Include(o => o.Courier)
                .SingleOrDefault(o => o.Id == id);

            if (order == null)
            {
                return null; // or throw an exception or handle as needed
            }

            var orderViewModel = new OrderViewModel
            {
                Id = order.Id,
                DeliveryStatus = order.DeliveryStatus,
                PaymentStatus = order.PaymentStatus,
                OrderTotal = order.OrderTotal,
                WeightTotal = order.WeightTotal,
                OrderDate = order.OrderDate,
                OrderItems = order.OrderItems?.Select(oi => new OrderItemViewModel
                {
                    OrderItemId = oi.Id,
                    FoodItemName = oi.FoodItem?.Name,
                    Amount = oi.Amount,
                    OrderItemTotal = oi.OrderItemTotal
                }).ToList(),
                AddressId = order.Address?.Id,
                CustomerId = order.Customer?.Id,
                CourierId = order.Courier?.Id
            };

            return orderViewModel;
        }
    }
}
