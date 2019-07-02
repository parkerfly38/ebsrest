using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class RMADetailsRequest
    {
        [MaxLength(3)]
        [Required]
        public string CompID { get; set; }

        [Required]
        public int RMAKey { get; set; }

        public string LoginName { get; set; }
    }
}