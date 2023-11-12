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
        public decimal Price { get; set; }
        public DateTime AddedDate { get; set; }


        public int? MenuId { get; set; }

        public int? CategoryId { get; set; }
        public IFormFile? Image { get; set; }

        public string? CategoryTitle { get; set; }
    }
}
