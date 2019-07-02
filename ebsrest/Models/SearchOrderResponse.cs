using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ebsrest.Models
{
    public class SearchOrderResponse
    {
        public List<SearchOrderLine> orders { get; set; }

        public int TotalRecords { get; set; }
    }

    public class SearchOrderLine
    {
        public int TranType { get; set; }

        public string CompanyID { get; set; }

        public string TranNo { get; set; }

        public string TranID { get; set; }

        public DateTime TranDate { get; set; }

        public decimal TranAmt { get; set; }

        public int SOKey { get; set; }

        public DateTime CreateDate { get; set; }

        public int CustKey { get; set; }

        public string CustPONo { get; set; }

        public string CurrID { get; set; }

        public string CustName { get; set; }

        public string CustID { get; set; }

        public string SperID { get; set; }

        public string SperName { get; set; }

        public string StatusDesc { get; set; }

        public string UnitMeasID { get; set; }

        public string ConfirmNo { get; set; }

        public string ShipToAddrName { get; set; }

        public decimal OpenAmt { get; set; }

        public decimal SalesAmt { get; set; }

        public string ItemID { get; set; }

        public decimal QtyOrd { get; set; }

        public string City { get; set; }

        public int SOLineKey { get; set; }

        public DateTime RequestDate { get; set; }

        public string ShortDesc { get; set; }

        public decimal QtyOnBO { get; set; }

        public decimal QtyOpenToShip { get; set; }

        public decimal QtyShip { get; set; }

        public int Hold { get; set; }

        public int SperKey { get; set; }

        public string TranDateYMD { get; set; }

        public int BlnktRelNo { get; set; }
    }
}