using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class GetSalesSourceIDsResponse
    {
        public int SalesSourceKey { get; set; }

        public string CompanyID { get; set; }

        public string Description { get; set; }

        public string SalesSourceID { get; set; }

        public int UpdateCounter { get; set; }
    }
}