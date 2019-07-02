using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ebsrest.Models
{
    public class InsertShipLineRequest
    {
        [Required]
        [MaxLength(3)]
        public string CompID { get; set; }

        [Required]
        public decimal QtyShip { get; set; }

        [Required]
        [MaxLength(20)]
        public string RefShipmentID { get; set; }

        [Required]
        [MaxLength(10)]
        public string SONumber { get; set; }

        [Required]
        public int SOLineNo { get; set; }

        [Required]
        public int SessionKey { get; set; }

        public string LoginName { get; set; }
    }
}