using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class ShoppingCartResponse
    {
        public List<SOOrders> orders { get; set; }

        public SOHeader header { get; set; }

        public List<SOBlanket> blanket { get; set; }

        public List<SOLine> lines { get; set; }

        public List<SOAddr> addr { get; set; }
    }
}