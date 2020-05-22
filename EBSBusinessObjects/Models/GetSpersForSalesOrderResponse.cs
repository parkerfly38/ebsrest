using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetSpersForSalesOrderResponse
    {
        public int CustKey { get; set; }

        public string CustId { get; set; }

        public string CustName { get; set; }

        public string IDName { get; set; }

        public string SperID { get; set; }

        public string SperName { get; set; }

        public int PrimarySperKey { get; set; }

        public string CompanyID { get; set; }
    }
}