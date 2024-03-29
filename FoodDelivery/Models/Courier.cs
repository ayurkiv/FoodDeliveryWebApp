﻿using FoodDelivery.Models.Common;
using FoodDelivery.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Models
{
    public class Courier : IModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public ICollection<Order>? Orders { get; set; }

        [EnumDataType(typeof(CourierStatus))]
        public CourierStatus CourierStatus {  get; set; }
    }

}
