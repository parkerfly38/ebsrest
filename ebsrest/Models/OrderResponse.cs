using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class OrderResponse
    {
        public int SOKey { get; set; }

        public string TranID { get; set; }

        public string Status { get; set; }

        public List<OrderError> Errors { get; set; }
    }
}