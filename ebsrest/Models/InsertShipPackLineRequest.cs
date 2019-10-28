using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class InsertShipPackLineRequest
    {
        [Required]
        [MaxLength(20)]
        public string RefShipmentID { get; set; }

        [Required]
        [MaxLength(30)]
        public string ItemID { get; set; }

        [Required]
        public decimal QtyShip { get; set; }

        [Required]
        public int PackageNo { get; set; }

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