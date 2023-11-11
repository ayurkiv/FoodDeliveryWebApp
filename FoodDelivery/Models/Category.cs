﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models
{
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ICollection<FoodItem>? FoodItems { get; set; }
        public int? MenuId { get; set; }
        public Menu? Menu { get; set; }

    }
}
