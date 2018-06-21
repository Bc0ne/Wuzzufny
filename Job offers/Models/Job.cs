using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Job_offers.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Job Name")]
        public String JobTitle { get; set; }

        [DisplayName("Job Description")]
        public string JobContent { get; set; }

        [DisplayName("Job Image")]
        public string JobImage { get; set; }

        [DisplayName("Job Type")]
        public int CategoryId { get; set; }

        public string UserID { get; set; }

        public virtual Category Category { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}