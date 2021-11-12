using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWasmAwsAmplify.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string StartLocation { get; set; }

        [Required]
        public string EndLocation { get; set; }
    }
}
