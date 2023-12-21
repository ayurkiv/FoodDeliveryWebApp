using FoodDelivery.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models
{
    public class OrderItem : IModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Amount { get; set; }

        public int OrderItemWeight { get; set; }

        public float OrderItemTotal { get; set; }

		public int? FoodItemId { get; set; }
		public FoodItem? FoodItem { get; set; }

		public int? OrderId { get; set; }
        public Order? Order { get; set; }

        public int? CartId { get; set; }
        public Cart? Cart { get; set; }

    }
}
