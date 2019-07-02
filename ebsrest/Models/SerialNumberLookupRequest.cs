using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class SerialNumberLookupRequest
    {
        [MaxLength(3)]
        [Required]
        public string CompID { get; set; }

        [MaxLength(30)]
        public string ItemID { get; set; }

        [MaxLength(20)]
        public string SerialNo { get; set; }

        public string LoginName { get; set; }
    }
}