using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EBSBusinessObjects.Models
{
    public class InsertShipDistRequest
    {
        [Required]
        [MaxLength(3)]
        public string CompID { get; set; }

        /// <summary>
        /// Bin ID from which Item is being oulled, required if Warehouse is tracked by bin.
        /// </summary>
        [MaxLength(20)]
        public string BinID { get; set; }

        [Required]
        public decimal DistQty { get; set; }

        [Required]
        [MaxLength(20)]
        public string RefShipmentID { get; set; }

        /// <summary>
        /// Lot Number required if lot controlled item
        /// </summary>
        public int? LotNo { get; set; }

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