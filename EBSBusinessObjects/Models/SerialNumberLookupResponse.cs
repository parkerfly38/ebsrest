using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class SerialNumberLookupResponse
    {
        public string ItemID { get; set; }

        public string WhseDesc { get; set; }

        public string WhseID { get; set; }

        public string ShortDesc { get; set; }

        public string CompanyID { get; set; }

        public string SerialNo { get; set; }

        public decimal DistQty { get; set; }

        public string Location { get; set; }

        public string WhseBinID { get; set; }

        public string InvtTranID { get; set; }

        public string RcvrDate { get; set; }

        public string RcvrTranID { get; set; }

        public string PODate { get; set; }

        public string POTranID { get; set; }

        public string SODate { get; set; }

        public string SOTranID { get; set; }

        public string INVDate { get; set; }

        public string INVTranID { get; set; }

        public int InvcKey { get; set; }

        public int SOKey { get; set; }

        public int POKey { get; set; }
        
        public int RcvrKey { get; set; }
    }
}