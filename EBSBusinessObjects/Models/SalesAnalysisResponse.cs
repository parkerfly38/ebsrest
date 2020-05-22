using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class SalesAnalysisResponse
    {
        public string Period { get; set; }

        public decimal SalesAmt { get; set; }

        public decimal COSAmt { get; set; }

        public decimal GrossProfit { get; set; }

    }
}