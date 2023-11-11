using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
    }
}
