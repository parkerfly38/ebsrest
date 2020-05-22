using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetShipmentsInvoicesRequest
    {
        [Required]
        [MaxLength(3)]
        public string CompID { get; set; }

        [MaxLength(12)]
        public string CustID { get; set; }

        public int? CustKey { get; set; }

        [MaxLength(30)]
        public string ItemID { get; set; }

        public int? ItemKey { get; set; }

        public int? FiscYear { get; set; }

        public int? FiscPer { get; set; }

        [MaxLength(1)]
        public string IorS { get; set; }

        public string LoginName { get; set; }

    }
}