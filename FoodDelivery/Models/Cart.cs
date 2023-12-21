﻿using FoodDelivery.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models
{
    public class Cart : IModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public float? CartTotal { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }


        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
		public Cart()
		{
			OrderItems = new List<OrderItem>();
		}
	}

}
