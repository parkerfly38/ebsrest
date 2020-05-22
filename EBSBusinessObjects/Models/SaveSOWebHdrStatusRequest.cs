using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class SaveSOWebHdrStatusRequest
    {
        [Required]
        public int SOKeyTemp { get; set; }

        public DateTime? OrderComplDate { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        public int SOKey { get; set; }

        [Required]
        public string CustID { get; set; }

        [Required]
        public string LoginName { get; set; }
    }
}