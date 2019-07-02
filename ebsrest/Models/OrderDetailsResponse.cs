using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class OrderDetailsResponse
    {
        public OrderDetailsHeader header { get; set; }

        public List<OrderDetailsLine> lines { get; set; }
    }

    public class OrderDetailsHeader
    {
        public int TranTypeNbr { get; set; }

        public string TranType { get; set; }

        public string TranNo { get; set; }

        public string TranID { get; set; }

        public string CustName { get; set; }

        public string CustID { get; set; }

        public string BillToAddr1 { get; set; }

        public string BillToAddr2 { get; set; }

        public string BillToAddr3 { get; set; }

        public string BillToAddr4 { get; set; }

        public string BillToAddr5 { get; set; }

        public string BillToCity { get; set; }

        public string BillToState { get; set; }

        public string BillToPostalCode { get; set; }

        public string BillToCountry { get; set; }

        public string ShipToCustID { get; set; }

        public string ShipToCustName { get; set; }

        public string ShipToAddr1 { get; set; }

        public string ShipToAddr2 { get; set; }

        public string ShipToAddr3 { get; set; }

        public string ShipToAddr4 { get; set; }

        public string ShipToAddr5 { get; set; }

        public string ShipToCity { get; set; }

        public string ShipToState { get; set; }

        public string ShipToPostalCode { get; set; }

        public string ShipToCountryID { get; set; }

        public string SalespersonID { get; set; }

        public string SalespersonName { get; set; }

        public string CustPONo { get; set; }

        public string Status { get; set; }

        public string Delivery { get; set; }

        public string OrderDate { get; set; }

        public string DfltRequestDate { get; set; }

        public string Expiration { get; set; }

        public decimal SalesAmt { get; set; }

        public decimal TotalAmt { get; set; }

        public string ContactName { get; set; }

        public string ContactPhone { get; set; }

        public string ContactPhoneExt { get; set; }

        public string ContactEmail { get; set; }

        public string SalesSource { get; set; }

        public string CustClassID { get; set; }

        public string CustClassName { get; set; }

        public string FreightMethod { get; set; }

        public string PmtTermsID { get; set; }

        public string PmtTermsDesc { get; set; }

        public string ConfirmNo { get; set; }

        public string User { get; set; }

        public string CurrID { get; set; }

        public string CloseDate { get; set; }

        public string Hold { get; set; }

        public string HoldReason { get; set; }

        public string Comment { get; set; }

        public string UserFld1 { get; set; }

        public string UserFld2 { get; set; }

        public string UserFld3 { get; set; }

        public string UserFld4 { get; set; }

        public string ShipMethID { get; set; }

        public decimal FreighAmt { get; set; }

        public decimal STaxAmt { get; set; }

        public decimal TradeDiscAmt { get; set; }

        public decimal TranAmt { get; set; }

        public string CompanyName { get; set; }

        public string CompanyAddrLine1 { get; set; }

        public string CompanyAddrLine2 { get; set; }

        public string CompanyAddrLine3 { get; set; }

        public string CompanyAddrLine4 { get; set; }

        public string CompanyCity { get; set; }

        public string CompanyStateID { get; set; }

        public string CompanyPostalCode { get; set; }

        public string CompanyCountryID { get; set; }

        public string CompanyPhone { get; set; }

        public int PmtTermsKey { get; set; }

        public string ExpDate { get; set; }

        public int DfltShipToAddrKey { get; set; }

        public int DfltShipToCAddrKey { get; set; }

        public int PrimarySperKey { get; set; }

        public int DfltWhseKey { get; set; }

        public string DfltWhseID { get; set; }

        public short TrackQtyAtBin { get; set; }
        
        public short BlnktRelNo { get; set; }

        public string WHAddrName { get; set; }

        public string WHAddrLine1 { get; set; }

        public string WHAddrLine2 { get; set; }

        public string WHAddrLine3 { get; set; }

        public string WHAddrLine4 { get; set; }

        public string WHAddrLine5 { get; set; }

        public string WHCity { get; set; }

        public string WHStateID { get; set; }

        public string WHPostalCode { get; set; }

        public string WHCountryID { get; set; }

        public string FOBPoint { get; set; }

        public string ContactFax { get; set; }
    }

    public class OrderDetailsLine
    {
        public int SOKey { get; set; }

        public int SOLineNo { get; set; }

        public int ItemKey { get; set; }

        public string Description { get; set; }

        public string ExtCmnt { get; set; }

        public string TranDate { get; set; }

        public short InclOnPackList { get; set; }

        public short InclOnPickList { get; set; }

        public string ItemID { get; set; }

        public decimal UnitPrice { get; set; }

        public string UnitMeasID { get; set; }

        public decimal ExtAmt { get; set; }

        public string ItemShortDesc { get; set; }

        public decimal OrigOrdered { get; set; }

        public decimal QtyORd { get; set; }

        public decimal QtyInvcd { get; set; }

        public decimal QtyOnBO { get; set; }

        public decimal QtyOpenToShip { get; set; }

        public string RequestDate { get; set; }

        public string OrigPromiseDate { get; set; }

        public string PromiseDate { get; set; }

        public string ShipDate { get; set; }

        public string ShipMethDesc { get; set; }

        public string ShipMethID { get; set; }

        public string STaxClassID { get; set; }

        public string CloseDate { get; set; }

        public short? CmntOnly { get; set; }

        public string CommClassID { get; set; }

        public string CommPlanDesc { get; set; }

        public short UnitPriceOvrd { get; set; }

        public string UserFld1 { get; set; }

        public string UserFld2 { get; set; }

        public decimal AmtInvcd { get; set; }

        public decimal FreightAmt { get; set; }

        public short Hold { get; set; }

        public string HoldReason { get; set; }

        public string FOBID { get; set; }

        public string FOBDesc { get; set; }

        public short ShipPriority { get; set; }

        public string CustAddrID { get; set; }

        public decimal TradeDiscAmt { get; set; }

        public decimal TradeDiscPct { get; set; }

        public string VendID { get; set; }

        public string VendName { get; set; }

        public string WhseDesc { get; set; }

        public string WhseID { get; set; }

        public int WhseKey { get; set; }

        public string TranID { get; set; }

        public int AddrKey { get; set; }

        public int CustKey { get; set; }

        public string CustID { get; set; }

        public string CustName { get; set; }

        public short Status { get; set; }

        public int SOLineKey { get; set; }

        public string CompanyID { get; set; }

        public short DeliveryMeth { get; set; }

        public int UnitMeasKey { get; set; }

        public decimal StdPrice { get; set; }

        public short TrackQtyAtBin { get; set; }

        public decimal ListPrice { get; set; }

        public short AllowDecimalQty { get; set; }

        public decimal UnitWeight { get; set; }

        public decimal ExtdWeight { get; set; }

        public int SOLineDistKey { get; set; }

    }
}