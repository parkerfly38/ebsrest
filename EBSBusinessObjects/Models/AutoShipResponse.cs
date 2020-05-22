using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class AutoShipResponse
    {
        public int TotalRecs { get; set; }

        public string Status { get; set; }

        public string StatusDetail { get; set; }
    }
}