using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class SORequest
    {
        [Required]
        public int SOKey { get; set; }

        [MaxLength(30)]
        public string CompID { get; set; }

        public string LoginName { get; set; }
    }
}