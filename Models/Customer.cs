using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorWasmAwsAmplify.Models
{
    public class Customer
    {
        public string CustomerId { get; set; } = "";

        public string MemberId { get; set; } = "";

        public string Email { get; set; } = "";

        public string Name { get; set; } = "";

        public string Romaji { get; set; } = "";

        public string City { get; set; } = "";

        public string Phone { get; set; } = "";

        public string Address { get; set; } = "";

        public string MembershipType { get; set; } = "";

        public string LastVisit { get; set; } = "";


    }
}
