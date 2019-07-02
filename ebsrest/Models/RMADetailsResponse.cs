using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class RMADetailsResponse
    {
        public SearchRMAResponse header { get; set; }

        public List<RMADetailsLine> lines { get; set; }

        public List<RMADetailsLineDist> lineDist { get; set; }
    }

    public class RMADetailsLine
    {
        public int RMALineKey { get; set; }

        public string CloseDate { get; set; }

        public int CommClassKey { get; set; }

        public int CommPlanKey { get; set; }

        public short? CustomBTOKit { get; set; }

        public string Description { get; set; }

        public decimal ExtAmt { get; set; }

        public string ExtCmnt { get; set; }

        public decimal FreightAmt { get; set; }

        public int ItemKey { get; set; }

        public int OrigShipLineKey { get; set; }

        public decimal QtyAuthForRtrn { get; set; }

        public int RcvgWhseKey { get; set; }

        public int ReasonCodeKey { get; set; }

        public decimal RestockAmt { get; set; }

        public int RMAKey { get; set; }

        public int RMALineNo { get; set; }

        public short RtrnType { get; set; }

        public int ShipLineKey { get; set; }

        public int ShipMethKey { get; set; }

        public int SOLineDistKey { get; set; }

        public short Status { get; set; }

        public int? STaxClassKey { get; set; }

        public int STaxTranKey { get; set; }

        public decimal TradeDiscAmt { get; set; }

        public int UnitMeasKey { get; set; }

        public decimal UnitPrice { get; set; }

        public short UnitPriceOvrd { get; set; }

        public int UpdateCounter { get; set; }

        public string UserFld1 { get; set; }

        public string UserFld2 { get; set; }

        public string UserFld3 { get; set; }

        public string UserFld4 { get; set; }

        public string ItemID { get; set; }
    }

    public class RMADetailsLineDist
    {
        public int RMALineDistKey { get; set; }

        public int RMALineKey { get; set; }

        public string ProposedSerialNo { get; set; }

    }
}