using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ebsrest.Models
{
    public class InsertShipRequest
    {
        [Required]
        [MaxLength(20)]
        public string RefBatchID { get; set; }

        [Required]
        [MaxLength(20)]
        public string RefShipmentID { get; set; }

        [Required]
        [MaxLength(15)]
        public string ShipMethID { get; set; }

        [Required]
        [MaxLength(10)]
        public string SONumber { get; set; }

        [Required]
        public DateTime TranDate { get; set; }

        [Required]
        public int SessionKey { get; set; }

        public string LoginName { get; set; }
    }
}