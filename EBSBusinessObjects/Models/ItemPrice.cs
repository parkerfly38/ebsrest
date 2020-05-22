using System;
using System.Collections.Generic;
using System.Text;

namespace EBSBusinessObjects.Models
{
    public class ItemPrice
    {
        public decimal UnitPrice { get; set; }

        public int PriceMethod { get; set; }

        public int SchedPriceDeterm { get; set; }

        public int SystemPriceDeterm { get; set; }

        public decimal UnitPriceFromSched { get; set; }

        public decimal UnitPriceFromPromo { get; set; }

        public string SalesPromotionID { get; set; }

        public int SalesPromotionKey { get; set; }

        public int ReturnValue { get; set; }
    }
}
