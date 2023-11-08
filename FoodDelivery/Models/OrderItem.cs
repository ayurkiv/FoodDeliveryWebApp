using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        [Required, MinLength(1)]
        public int Amount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal OrderItemTotal { get; set; }



        public int FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; }

        public int OrderId { get; set; }


    }

}
