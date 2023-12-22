using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.ViewModels
{
    public class PaymentViewModel
    {
        [Required]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Card Number is required")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Card Number must be 16 digits")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Card Number must contain only digits")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Expiration Date is required")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Invalid Expiration Date. Use format MM/YY")]
        public string ExpirationDate { get; set; }

        [Required(ErrorMessage = "CVV is required")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "CVV must be 3 digits")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "CVV must contain only digits")]
        public string CVV { get; set; }
    }
}
