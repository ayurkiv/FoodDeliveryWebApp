using System.Data;
using System.ComponentModel.DataAnnotations;

namespace Delivery.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public int AdressId { get; set; }
        public Address Address { get; set; }
        public List<Order> Orders { get; set; }
    }

    public class Address
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string City { get; set; }
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public User User { get; set; }
    }

    public class Courier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public bool IsBusy { get; set; }
    }

    public class Admin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public List<FoodItem> FoodItems { get; set; }
    }

    public class FoodItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int AddedByAdminId { get; set; }
        public Admin AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public List<Menu> Menus { get; set; }
    }

    public class Menu
    {
        public int Id { get; set; }
        public int FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; }
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; }
        public int Amount { get; set; }
        public decimal OrderItemTotal { get; set; }
        public Order Order { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CourierId { get; set; }
        public Courier Courier { get; set; }
        public int OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
        public string OrderStatus { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }

    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
    }
}
