using FoodDelivery.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.ViewModel
{
    public class FoodItemViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public bool Available { get; set; }
        public int TimeToReady { get; set; }
        public int Weight { get; set; }

        public float Price { get; set; }
        public DateTime AddedDate { get; set; }

        public int? CategoryId { get; set; }
        public IFormFile? Image { get; set; }

        public string? ImageUrl { get; set; }

        [DisplayName("Category")]
        public string? CategoryName { get; set; }

    }
}
