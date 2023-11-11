using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models
{
    public class FoodItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Amount { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        public DateTime AddedDate { get; set; }


        public int? MenuId { get; set; }
        public Menu? Menu { get; set; }

        public int? OrderItemId { get; set; }
        public OrderItem? OrderItem { get; set; }

        public FoodItem() => AddedDate = DateTime.Now;
    }
}
