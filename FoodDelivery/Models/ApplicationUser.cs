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
        [Key]
        public int ApplicationUserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }


        public int? CourierId { get; set; } 
        public Courier? Courier { get; set; }

        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }

    }
}
