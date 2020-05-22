using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetRecurBlnktOrdersResponse
    {
        public List<BlnktOrder> orders { get; set; }

        public List<BlnktOrder> lines { get; set; }
    }

    public class BlnktOrder {

        public string ItemID { get; set; }

        public decimal QtyOrd { get; set; }

        public decimal QtyOpenToShip { get; set; }

        public decimal UnitPrice { get; set; }

        public string UnitMeasID { get; set; }

        public string ShipDate { get; set; }

        public string ShipMethIDLn { get; set; }

        public int SOLineNo { get; set; }

        public string ShortDesc { get; set; }

        public int DWhseKey { get; set; }

        public int SOLineKey { get; set; }

        public string CustPONo { get; set; }

        public int SOKey { get; set; }

        public short Priority { get; set; }

        public string TranID { get; set; }

        public string TranNo { get; set; }

        public string TranDate { get; set; }

        public int DfltShipMethKey { get; set; }

        public string ShipMethID { get; set; }

        public string ShipMethDesc { get; set; }

        public short DfltShipPriority { get; set; }

        public int DfltWhseKey { get; set; }

        public string WhseID { get; set; }

        public string WhseDesc { get; set; }

        public string CustID { get; set; }

        public string CustName { get; set; }

        public int DfltShipToAddrKey { get; set; }

        public string ShipToAddrName { get; set; }

        public string ShipToAddrLine1 { get; set; }

        public string ShipToCity { get; set; }

        public string ShipToStateID { get; set; }

        public string ShipFromAddrName { get; set; }

        public string ShipFromAddrLine1 { get; set; }

        public string ShipFromCity { get; set; }

        public string ShipFromStateID { get; set; }

        public string PRocCycleID { get; set; }

        public string LastSODate { get; set; }

        public short NoSOReleased { get; set; }

        public decimal ReleasedToDate { get; set; }

        public string StartDate { get; set; }

        public string StopDate { get; set; }

        public int PrimarySperKey { get; set; }

        public int CustKey { get; set; }
    }
}