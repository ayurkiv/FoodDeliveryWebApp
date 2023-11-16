using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models
{
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? OrderStatus { get; set; }

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
