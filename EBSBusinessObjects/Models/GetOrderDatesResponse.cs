using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetOrderDatesResponse
    {
        public DateTime RequestDate { get; set; }

        public DateTime PromiseDate { get; set; }

        public DateTime ShipDate { get; set; }
    }
}