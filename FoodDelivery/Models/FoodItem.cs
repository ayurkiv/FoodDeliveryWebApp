using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models
{
    public class FoodItem
    {
        [Key]
        public int FoodItemId { get; set; }
        public string? Description { get; set; }
        public int Amount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public DateTime AddedDate { get; set; }


        [ForeignKey(nameof(Menu))]
        public int MenuId { get; set; }
        public Menu Menu { get; set; }


        public OrderItem OrderItem { get; set; }
    }


}
