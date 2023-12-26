using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FoodDelivery.Models.Interfaces;
using FoodDelivery.Models.Common;

namespace FoodDelivery.Models
{
    public class Category : ISoftDeletable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Title { get; set; }
        public ICollection<FoodItem>? FoodItems { get; set; }

        public bool IsDelete { get; set; }

        public Category()
        {
            IsDelete = false;
        }

    }
}
