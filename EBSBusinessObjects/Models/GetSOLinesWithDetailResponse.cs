using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetSOLinesWithDetailResponse
    {
        public int SOLineNo { get; set; }

        public string ShortDesc { get; set; }

        public string LongDesc { get; set; }

        public string ItemID { get; set; }

        public decimal QtyOrd { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal ExtAmt { get; set; }

        public string ItemClassID { get; set; }

        public string ItemClassName { get; set; }

        public decimal QtyOpenToShip { get; set; }
    }
}