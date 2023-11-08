using FoodDelivery.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Customer
{
    [Key]
    public int CustomerId { get; set; }

    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public int AddressId { get; set; }
    public Address Address { get; set; }

    public int OrderId { get; set; }

}

