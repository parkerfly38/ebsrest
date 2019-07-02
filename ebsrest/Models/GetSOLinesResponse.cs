using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class GetSOLinesResponse
    {
        public int SOLineNo { get; set; }

        public string LongDesc { get; set; }

        public string ItemID { get; set; }

        public decimal QtyOrd { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal ExtAmt { get; set; }
    }
}