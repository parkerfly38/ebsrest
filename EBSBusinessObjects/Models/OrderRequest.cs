using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class OrderRequest
    {
        [MaxLength(10)]
        public string OrderID { get; set; }

        public DateTime AckDate { get; set; }

        [MaxLength(12)]
        [Required]
        public string CustID { get; set; }

        [MaxLength(15)]
        public string CustPONo { get; set; }

        public DateTime RequestDate { get; set; }

        public string ShipMethID { get; set; }

        public string ShipToName { get; set; }

        [MaxLength(40)]
        public string ShipToAddrLine1 { get; set; }

        [MaxLength(40)]
        public string ShipToAddrLine2 { get; set; }

        [MaxLength(40)]
        public string ShipToAddrLine3 { get; set; }

        [MaxLength(40)]
        public string ShipToAddrLine4 { get; set; }

        [MaxLength(40)]
        public string ShipToAddrLine5 { get; set; }

        [MaxLength(20)]
        public string ShipToCity { get; set; }
        
        [MaxLength(3)]
        public string ShipToCountryID { get; set; }

        [MaxLength(9)]
        public string ShipToPostalCode { get; set; }

        [MaxLength(3)]
        public string ShipToStateID { get; set; }

        [MaxLength(50)]
        public string TranCmnt { get; set; }

        [MaxLength(15)]
        public string UserFld1 { get; set; }

        [MaxLength(15)]
        public string UserFld2 { get; set; }

        [MaxLength(15)]
        public string UserFld3 { get; set; }

        [MaxLength(15)]
        public string UserFld4 { get; set; }

        public string CompID { get; set; }

        public int sessKey { get; set; }

        public int tranType { get; set; }

        public string TranTypeString { get; set; }

        [MaxLength(15)]
        public string PmtTermsID { get; set; }

        public int PmtTermsKey { get; set; }

        public DateTime ExpirationDate { get; set; }

        public decimal TaxAmt { get; set; }

        public decimal FreightAmt { get; set; }

        public string FreightAllocMethQorA { get; set; }

        public decimal TradeDiscAmt { get; set; }

        public decimal TradeDiscPct { get; set; }

        public string CalcSalesTax { get; set; }

        public int Hold { get; set; }

        public int DfltSperKey { get; set; }

        public string SalesSourceID { get; set; }

        public string HoldReason { get; set; }

        public int Status { get; set; }

        public string BillToAddrLine1 { get; set; }

        public string BillToAddrLine2 { get; set; }

        public string BillToAddrLine3 { get; set; }

        public string BillToAddrLine4 { get; set; }

        public string BillToAddrLine5 { get; set; }

        public string BillToName { get; set; }

        public string BillToCity { get; set; }

        public string BillToCountryID { get; set; }

        public string BillToPostalCode { get; set; }

        public string BillToStateID { get; set; }

        public int DfltWhseKey { get; set; }

        public string CreateUserID { get; set; }

        public int STaxSchdKey { get; set; }

        public int? CrHold { get; set; }

        public string CRMOpportunityID { get; set; }

        public short isDropShip { get; set; }

        public string FOB { get; set; }

        public int FOBKey { get; set; }

        public string ShipToAddrID { get; set; }

        public string LoginName { get; set; }

        public int? DfltDelMethod { get; set; }

        public List<OrderLineRequest> lines { get; set; }
    }
}