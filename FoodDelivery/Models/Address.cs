namespace FoodDelivery.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
