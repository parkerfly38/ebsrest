using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class SOHeader
    {
        public string CompID { get; set; }

        public int SOKeyTemp { get; set; }

        public string PortalSessionID { get; set; }

        public string LoginName { get; set; }

        public DateTime OrderInitDate { get; set; }

        public DateTime? OrderComplDate { get; set; }

        public int Status { get; set; }

        public int OrderType { get; set; }

        public string CustID { get; set; }

        public string CustPO { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime? ExpDate { get; set; }

        public DateTime? ShipDate { get; set; }

        public DateTime? PromiseDate { get; set; }

        public string ShipMethID { get; set; }

        public int OverrideShipAddr { get; set; }

        public int AddrKey { get; set; }

        public string ShipFromAddrName { get; set; }

        public string ShipFromAddrLine1 { get; set; }

        public string ShipFromAddrLine2 { get; set; }

        public string ShipFromAddrLine3 { get; set; }

        public string ShipFromAddrLine4 { get; set; }

        public string ShipFromAddrLine5 { get; set; }

        public string ShipFromCity { get; set; }

        public string ShipFromStateID { get; set; }

        public string ShipFromPostalCode { get; set; }

        public string ShipFromCountryID { get; set; }

        public string ShipToAddrName { get; set; }

        public string ShipToAddrLine1 { get; set; }

        public string ShipToAddrLine2 { get; set; }

        public string ShipToAddrLine3 { get; set; }

        public string ShipToAddrLine4 { get; set; }

        public string ShipToAddrLine5 { get; set; }

        public string ShipToCity { get; set; }

        public string ShipToStateID { get; set; }

        public string ShipToPostalCode { get; set; }

        public string ShipToCountryID { get; set; }

        public string ClosestWhse { get; set; }

        public int ShipDays { get; set; }

        public string SalesPersonID { get; set; }

        public string Currency { get; set; }

        public string Comment { get; set; }

        public string UserFld1 { get; set; }

        public string UserFld2 { get; set; }

        public string UserFld3 { get; set; }

        public string UserFld4 { get; set; }

        public string OrderID { get; set; }

        public int TotalLines { get; set; }

        public decimal TotalExtdAmt { get; set; }

        public decimal TotalTaxable { get; set; }

        public decimal TotalExemption { get; set; }

        public decimal TotalTax { get; set; }

        public string PmtTermsID { get; set; }

        public int STaxSchdKey { get; set; }

        public string CustName { get; set; }

        public decimal TradeDiscAmt { get; set; }

        public decimal FreightAmt { get; set; }

        public decimal TradeDiscPct { get; set; }

        public int BillToAddrKey { get; set; }

        [MaxLength(100)]
        public string ImportRef { get; set; }

        public string ShipStatus { get; set; }

        public string SalesSourceID { get; set; }

        public int isDropShip { get; set; }

        public string DfltFOBID { get; set; }

        public int DfltFOBKey { get; set; }

        [MaxLength(1)]
        public string UseCC { get; set; }

        [MaxLength(1)]
        public string Hold { get; set; }

        /// <summary>
        /// (S)ave or (D)elete a sales order
        /// </summary>
        [MaxLength(1)]
        public string SorD { get; set; }
    }
}