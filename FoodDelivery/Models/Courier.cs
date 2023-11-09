using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models
{
    public class Courier
    {
        [Key]
        public int CourierId { get; set; }


        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int? OrderId { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }

}
