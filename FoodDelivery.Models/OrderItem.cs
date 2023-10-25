using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public int FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; }

        [Required, MinLength(1)]
        public int Amount { get; set; }
        public decimal OrderItemTotal { get; set; }
    }
}
