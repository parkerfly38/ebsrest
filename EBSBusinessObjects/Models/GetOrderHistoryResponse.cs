using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetOrderHistoryResponse
    {
        public string InvcNo { get; set; }

        public string InvcDate { get; set; }

        public decimal QtyShipped { get; set; }

        public decimal UnitCost { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal ExtAmt { get; set; }

        public string CompanyID { get; set; }

        public int? ItemKey { get; set; }

        public string CustName { get; set; }
    }
}