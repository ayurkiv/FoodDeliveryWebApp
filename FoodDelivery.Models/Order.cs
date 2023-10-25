using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourierId { get; set; }
        public int OrderItemId { get; set; }
        public int AddressId { get; set; }
        public int PaymentId { get; set; }
        public string OrderStatus { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime OrderDate { get; set; }
        public User User { get; set; }
        public Courier Courier { get; set; }
        public OrderItem OrderItem { get; set; }
        public Address Address { get; set; }
        public Payment Payment { get; set; }
    }
}
