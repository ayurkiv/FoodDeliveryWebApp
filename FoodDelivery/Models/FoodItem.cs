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
        public string? Image {  get; set; }

        public bool Available { get; set; }
        public int TimeToReady { get; set; }
        public int Weight {  get; set; }

        public float Price { get; set; }
        public DateTime AddedDate { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public int? OrderItemId { get; set; }
        public OrderItem? OrderItem { get; set; }

        public FoodItem()
        {
            AddedDate = DateTime.Now;
            Available = true;
        }
    }
}
