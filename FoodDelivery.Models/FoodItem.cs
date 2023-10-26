using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Models
{
    public class FoodItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public int AddedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime AddedDate { get; set; }
        public Admin AddedByAdmin { get; set; }

    }
}
