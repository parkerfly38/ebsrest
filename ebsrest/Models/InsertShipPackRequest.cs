using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class InsertShipPackRequest
    {
        [Required]
        [MaxLength(20)]
        public string RefBatchID { get; set; }

        [Required]
        [MaxLength(20)]
        public string RefShipmentID { get; set; }

        [Required]
        [MaxLength(30)]
        public string CartonID { get; set; }

        [MaxLength(2000)]
        public string Comment { get; set; }

        public decimal? FrtAmt { get; set; }

        [Required]
        [MaxLength(15)]
        public string FreightClassID { get; set; }

        public string ShipTrackNo { get; set; }

        [Required]
        public int PackageNo { get; set; }

        [Required]
        [MaxLength(10)]
        public string SONumber { get; set; }

        [Required]
        public int SessionKey { get; set; }

        public string LoginName { get; set; }
    }
}