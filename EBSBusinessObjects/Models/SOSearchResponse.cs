using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class SOSearchResponse
    {
        public int Row { get; set; }

        public int TotalCount { get; set; }

        public int SOKey { get; set; }

        public int TranType { get; set; }

        public string CustID { get; set; }

        public int CustKey { get; set; }

        public string CustName { get; set; }

        public string TranNo { get; set; }

        public string TranID { get; set; }

        public string TranDate { get; set; }

        public string CustPONo { get; set; }

        public string StatusDesc { get; set; }

        public string CreateDate { get; set; }

        public string SperID { get; set; }

        public decimal OrderTotal { get; set; }

        public string PONumbers { get; set; }
    }
}