using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetShipmentForRMARequest
    {
        [MaxLength(3)]
        public string CompID { get; set; }

        [MaxLength(10)]
        public string CustID { get; set; }

        [MaxLength(15)]
        public string ShipToAddrID { get; set; }

        [MaxLength(15)]
        public string BillToAddrID { get; set; }

        [MaxLength(10)]
        public string SOTranNo { get; set; }

        public int? SOLineNo { get; set; }

        [MaxLength(30)]
        public string ItemID { get; set; }

        [MaxLength(30)]
        public string TrackNo { get; set; }

        [MaxLength(10)]
        public string SHTranNo { get; set; }

        public string LoginName { get; set; }
    }
}