using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? StreetNumber { get; set; }

        [Required]
        public Customer Customer { get; set; }
    }
}
