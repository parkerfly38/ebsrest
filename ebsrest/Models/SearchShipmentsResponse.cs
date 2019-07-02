using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class SearchShipmentsResponse
    {
        public List<SearchShipment> shipments { get; set; }

        public int ResultSize { get; set; }
    }

    public class SearchShipment
    {

        public int Row { get; set; }

        public string CompanyID { get; set; }

        public int ShipKey { get; set; }

        public string CreateDate { get; set; }

        public int CustKey { get; set; }

        public int DlvryShipMethKey { get; set; }

        public decimal FreightAmt { get; set; }

        public string PostDate { get; set; }

        public int ShipToAddrKey { get; set; }

        public int ShipToCustAddrKey { get; set; }

        public string TrailerNo { get; set; }

        public string TranCmnt { get; set; }

        public string TranDate { get; set; }

        public int TranType { get; set; }

        public string TranID { get; set; }

        public string TranNo { get; set; }

        public int WhseKey { get; set; }

        public string CustID { get; set; }

        public string ShipMethDesc { get; set; }

        public string ShipMethID { get; set; }

        public string ShipName { get; set; }

        public string ShipAddr1 { get; set; }

        public string ShipAddr2 { get; set; }

        public string ShipAddr3 { get; set; }

        public string ShipAddr4 { get; set; }

        public string ShipAddr5 { get; set; }

        public string ShipCity { get; set; }

        public string ShipState { get; set; }

        public string ShipZip { get; set; }

        public string ShipCountry { get; set; }

        public string WhseID { get; set; }

        public string TranDateYMD { get; set; }

        public string CustName { get; set; }

        public string TrackingNumbers { get; set; }

        public string SalesOrders { get; set; }

        public string CustPOs { get; set; }

        public string SalesOrderIDs { get; set; }

        public string SalesOrderNos { get; set; }

        public string InvoiceIDs { get; set; }

        public int PrimarySperKey { get; set; }
    }
}