using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorWasmAwsAmplify.Models
{
    public class User
    {
        [Required]
        public string Name { get; set; } = "";
        [Required]
        public string City { get; set; } = "";
        [Required]
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";


    }
}
