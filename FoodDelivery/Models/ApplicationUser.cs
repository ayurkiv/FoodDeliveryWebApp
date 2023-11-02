using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int ApplicationUserId { get; set; }
        public string UserSurname { get; set; }
        public int? AddressId { get; set; } // Змініть тип на int?, щоб дозволити null
        public Address Address { get; set; }
    }
}
