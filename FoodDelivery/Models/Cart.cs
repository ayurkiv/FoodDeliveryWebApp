using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models
{
    public class Cart
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? OrderTotal { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }

        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }

}
