using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        [Required, MinLength(1)]
        public int Amount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal OrderItemTotal { get; set; }

        [Required]
        public FoodItem FoodItem { get; set; }

        public int? OrderId { get; set; }
        public Order? Order { get; set; }

    }

}
