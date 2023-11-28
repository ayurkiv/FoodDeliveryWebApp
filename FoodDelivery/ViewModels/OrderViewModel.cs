using FoodDelivery.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.ViewModel
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string? DeliveryStatus { get; set; }
        public string? PaymentStatus { get; set; }

        public float? OrderTotal { get; set; }
        public int? WeightTotal { get; set; }
        public DateTime? OrderDate { get; set; }

        public List<OrderItemViewModel>? OrderItems { get; set; }

        public int? AddressId { get; set; }
        public Address? Address { get; set; }

        public int? CustomerId { get; set; }

        public int? CourierId { get; set; }

    }
}
