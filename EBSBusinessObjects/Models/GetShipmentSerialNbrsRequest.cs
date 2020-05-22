using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetShipmentSerialNbrsRequest
    {
        [MaxLength(3)]
        public string CompID { get; set; }

        public int? ShipLineKey { get; set; }

        public int? ShipLineDistKey { get; set; }

        [MaxLength(10)]
        public string ShipmentNbr { get; set; }

        public string LoginName { get; set; }
    }
}