using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models
{
    public class AddressViewModel
    {
        public string? City { get; set; }

        public string? Street { get; set; }

        public string? StreetNumber { get; set; }
    }
}
