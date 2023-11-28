using FoodDelivery.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models
{
    public class CartViewModel
    {
        public int Id { get; set; }

        public float? CartTotal { get; set; }

		public List<OrderItemViewModel> OrderItems { get; set; }

		public int? CustomerId { get; set; }
    }

}
