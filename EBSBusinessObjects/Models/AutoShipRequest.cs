using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class AutoShipRequest
    {
        [Required]
        [MaxLength(3)]
        public string CompanyID { get; set; }

        [Required]
        public int SessionKey { get; set; }

        [MaxLength(50)]
        public string LoginName { get; set; }
    }
}