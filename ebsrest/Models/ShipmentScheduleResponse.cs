using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class ShipmentScheduleResponse
    {
        public string ShipDate { get; set; }

        public string UserFld1 { get; set; }

        public string UserFld2 { get; set; }

        public string ExtCmnt { get; set; }

        public string ShipMethID { get; set; }

        public string RequestDate { get; set; }

        public string PromiseDate { get; set; }

        public string AddrLine1 { get; set; }

        public string AddrLine2 { get; set; }

        public string AddrLine3 { get; set; }

        public string City { get; set; }

        public string StateID { get; set; }

        public string PostalCode { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string ShipZoneID { get; set; }

        public string ItemID { get; set; }

        public decimal QtyOrd { get; set; }

        public string UnitMeasID { get; set; }

        public decimal QOH { get; set; }

        public string map { get; set; }

        public int SOLineKey { get; set; }

        public int SOLineDistKey { get; set; }

        public int SOKey { get; set; }

        public string WhseID { get; set; }

        public string LineDesc { get; set; }

    }
}