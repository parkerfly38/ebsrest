using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class GetShipmentSerialNbrsResponse
    {
        public int ItemKey { get; set; }

        public string ItemID { get; set; }

        public string SerialNo { get; set; }

        public int InvtSerialKey { get; set; }

        public int ShipKey { get; set; }

        public string ShipmentNbr { get; set; }

        public string ShipmentID { get; set; }

        public string CompanyID { get; set; }
    }
}