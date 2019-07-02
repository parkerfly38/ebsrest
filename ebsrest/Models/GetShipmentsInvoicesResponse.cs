using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class GetShipmentsInvoicesResponse
    {
        public string CompanyID { get; set; }

        public int? FiscYear { get; set; }

        public int? FiscPer { get; set; }

        public string ItemID { get; set; }

        public int? ItemKey { get; set; }

        public string CustID { get; set; }

        public int? CustKey { get; set; }

        public decimal? QtyInvoiced { get; set; }

        public decimal? AmtInvoiced { get; set; }

    }
}