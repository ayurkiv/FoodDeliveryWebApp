using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models
{
    public class Menu
    {
        [Key]
        public int MenuId { get; set; }

        public int? FoodItemId { get; set; }
        public ICollection<FoodItem>? FoodItems { get; set; }

    }
}
