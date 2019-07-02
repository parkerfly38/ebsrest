using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class GetShipmentForRMAResponse
    {
        public string SOTranID { get; set; }

        public int SOLineNo { get; set; }

        public string ItemID { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal QtyShip { get; set; }

        public decimal MaxRtnQty { get; set; }

        public string UnitMeasID { get; set; }

        public string SHTranNo { get; set; }

        public string SHTranID { get; set; }

        public string ShipTrackNo { get; set; }

        public string ShortDesc { get; set; }

        public string ShipToCustAddrID { get; set; }

        public string BillToCustAddrID { get; set; }

        public string CurrID { get; set; }

        public decimal CurrExchRate { get; set; }

        public string RcvgWhseID { get; set; }

        public int ItemKey { get; set; }

        public int ShipLineKey { get; set; }

        public int RcvgWhseKey { get; set; }

        public int SOLineDistKey { get; set; }

        public int STaxClassKey { get; set; }

        public int UnitMeasKey { get; set; }

        public decimal RestockRate { get; set; }

        public decimal QtyAuthForRtrn { get; set; }

        public string ShipDate { get; set; }
    }
}