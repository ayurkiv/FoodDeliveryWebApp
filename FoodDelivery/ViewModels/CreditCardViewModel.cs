using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.ViewModels
{
    public class CreditCardViewModel
    {
        [Required]
        public string CardNumber { get; set; }

        [Required]
        public string ExpiryDate { get; set; }

        [Required]
        public string CVV { get; set; }
    }
}
