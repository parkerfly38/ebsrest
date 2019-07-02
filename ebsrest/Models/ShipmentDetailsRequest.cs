using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class ShipmentDetailsRequest
    {
        [MaxLength(3)]
        [Required]
        public string CompID { get; set; }

        [Required]
        public int ShipKey { get; set; }

        public string LoginName { get; set; }
    }
}