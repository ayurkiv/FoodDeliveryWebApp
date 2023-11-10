using FoodDelivery.Models;
using System.ComponentModel.DataAnnotations;

public class Customer
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string ApplicationUserId { get; set; }
    [Required]
    public ApplicationUser ApplicationUser { get; set; }

    public int? AddressId { get; set; }
    public Address? Address { get; set; }

    public ICollection<Order>? Orders { get; set; }
}

