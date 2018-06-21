using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Job_offers.Models
{
    public class Contact
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Subbject { get; set; }
        [Required]
        public string Message { get; set; }
    }
}