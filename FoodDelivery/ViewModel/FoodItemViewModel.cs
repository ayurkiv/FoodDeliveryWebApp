using FoodDelivery.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.ViewModel
{
    public class FoodItemViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Amount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        public DateTime AddedDate { get; set; }


        public int? MenuId { get; set; }
        public Menu? Menu { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public int? OrderItemId { get; set; }
        public OrderItem? OrderItem { get; set; }
    }
}
