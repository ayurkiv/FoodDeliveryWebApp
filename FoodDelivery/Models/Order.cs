using FoodDelivery.Models.Common;
using FoodDelivery.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models
{
    public class Order : IModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [EnumDataType(typeof(DeliveryStatus))]
        public DeliveryStatus DeliveryStatus { get; set; }

        [EnumDataType(typeof(PaymentStatus))]
        public PaymentStatus PaymentStatus { get; set; }

        public float? OrderTotal { get; set; }
        public int? WeightTotal { get; set; }
        public DateTime? OrderDate { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }

        public int? AddressId { get; set; }
        public Address? Address { get; set; }

        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public int? CourierId { get; set; }
        public Courier? Courier { get; set; }

        public Order ()
        {
            OrderDate = DateTime.Now;
        }
    }

}
