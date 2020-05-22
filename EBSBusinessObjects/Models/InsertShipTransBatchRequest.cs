using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EBSBusinessObjects.Models
{
    public class InsertShipTransBatchRequest
    {
        /// <summary>
        /// Begin point for Shipment
        /// 1 = After SO Create
        /// 2 = After Create Picks
        /// 3 = After Confirm Picks
        /// </summary>
        [Required]
        public int BeginPoint { get; set; }

        /// <summary>
        /// End point for Shipment
        /// 1 = After Shipments are generated
        /// 2 = After Shipments are committed
        /// 3 = After IM is posted
        /// 4 = After invoices are pulled into AR Batch
        /// 5 = After invoices are posted
        /// </summary>
        [Required]
        public int EndPoint { get; set; }

        [Required]
        public DateTime PostDate { get; set; }
        
        /// <summary>
        /// Valud value for Company for Segment from AR options, optional
        /// </summary>
        [MaxLength(15)]
        public string OvrdSegValue { get; set; }

        /// <summary>
        /// Batch ID required
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string RefBatchID { get; set; }

        /// <summary>
        /// Valid Sage 500 Warehouse, required.
        /// </summary>
        [Required]
        [MaxLength(6)]
        public string WhseID { get; set; }

        /// <summary>
        /// Unique ID for this Session of AutoShip
        /// </summary>
        [Required]
        public int SessionKey { get; set; }

        [MaxLength(50)]
        public string ARBatchCmnt { get; set; }

        public int? ARBatchKey { get; set; }

        public string LoginName { get; set; }
    }
}