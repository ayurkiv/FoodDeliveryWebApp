using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models
{
    public class CartViewModel
    {
        public int Id { get; set; }

        public float? OrderTotal { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }

        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }

}
