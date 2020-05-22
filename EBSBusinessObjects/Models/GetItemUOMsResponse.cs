using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetItemUOMsResponse
    {
        public int TargetUnitMeasKey { get; set; }

        public string UnitMeasID { get; set; }

        public int ConversionFactor { get; set; }

        public int UnitMeasKey { get; set; }

        public string ItemID { get; set; }

        public string UPC { get; set; }

        public float UnitVolume { get; set; }

        public float UnitWeight { get; set; }
    }
}