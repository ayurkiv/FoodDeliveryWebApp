using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public string? OrderStatus { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal OrderTotal { get; set; }
        public DateTime OrderDate { get; set; }




        [ForeignKey(nameof(Address))]
        public int AddressId { get; set; }
        public Address? Address { get; set; }


        [ForeignKey("OrderId")]
        public ICollection<OrderItem> OrderItems { get; set; }


        
    }

}
