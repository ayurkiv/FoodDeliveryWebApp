﻿using System;
using System.Collections.Generic;
using System.Linq;
using FoodDelivery.Data;
using FoodDelivery.Models;
using FoodDelivery.Models.Common;
using FoodDelivery.ViewModel;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

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

        public async Task<bool> AddOrderItemsToOrderAsync(int orderId, List<OrderItem> orderItems)
        {
            var existingOrder = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (existingOrder == null || orderItems == null || orderItems.Count == 0)
            {
                return false; // Order not found or no order items provided
            }

            // Create a separate list to avoid modifying the original collection during iteration
            var itemsToAdd = new List<OrderItem>();

            foreach (var orderItem in orderItems)
            {
                orderItem.OrderId = orderId;
                itemsToAdd.Add(orderItem);
            }

            // Add all order items to the order
            existingOrder.OrderItems.AddRange(itemsToAdd);

            await _context.SaveChangesAsync();

            return true;
        }

        public void UpdateOrderPaymentStatus(OrderViewModel order)
        {
            var existingOrder = _context.Orders.Find(order.Id);

            if (existingOrder == null)
            {
                // Можна кинути виняток або взяти інші заходи залежно від ваших потреб
                throw new InvalidOperationException($"Order with ID {order.Id} not found");
            }

            // Оновлення всіх полів
            existingOrder.PaymentStatus = order.PaymentStatus;
            // Оновлення зв'язаних об'єктів (якщо вони були змінені)

            // Оновіть тут інші властивості замовлення за потребою

            _context.Orders.Update(existingOrder);
            _context.SaveChanges();
        }

        public async Task<bool> AssignCourierToOrderAsync(int orderId)
        {
            var order = await _context.Orders.Include(o => o.Courier)
                                              .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null || order.Courier != null || order.DeliveryStatus != DeliveryStatus.Pending)
            {
                return false; // Order not found, already has a courier, or not in pending status
            }

            while (true)
            {
                var freeCourier = await GetFreeCourierAsync();

                if (freeCourier != null)
                {
                    order.Courier = freeCourier;
                    freeCourier.CourierStatus = CourierStatus.Busy;

                    await _context.SaveChangesAsync();

                    return true; // Courier successfully assigned
                }

                // Add a delay or implement a backoff strategy if needed
                await Task.Delay(TimeSpan.FromSeconds(5)); // Example: wait for 5 seconds before retrying
            }
        }

        private async Task<Courier> GetFreeCourierAsync()
        {
            Courier freeCourier = null;

            // Loop until a free courier is found
            while (freeCourier == null)
            {
                freeCourier = await _context.Couriers.FirstOrDefaultAsync(c => c.CourierStatus == CourierStatus.Free);

                if (freeCourier == null)
                {
                    // Add a delay or implement a backoff strategy if needed
                    await Task.Delay(TimeSpan.FromSeconds(5)); // Example: wait for 5 seconds before retrying
                }
            }

            return freeCourier;
        }

    }
}
