using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class GetSOOptionsResponse
    {
        public string CompanyID { get; set; }

        public short ActivateOutOfStockManager { get; set; }

        public short AllowDiscontItemRtrns { get; set; }

        public short AllowDropShipRtrn { get; set; }

        public short AllowExpiredLotOnRtrn { get; set; }

        public short AllowMultSOPerShip { get; set; }

        public short AutoSubstInImmedPick { get; set; }

        public short AutoSubstInPick { get; set; }

        public short CommitSOLine { get; set; }

        public short CreditCheckAtOrderEntry { get; set; }

        public short CreditCheckAtPicking { get; set; }

        public short CreditHoldAutoRelease { get; set; }

        public decimal CreditLimitTol { get; set; }

        public int DfltBOLFormKey { get; set; }

        public int DfltQuoteFormKey { get; set; }

        public int DfltSOAckFormKey { get; set; }

        public short DfltSpecItemtoDS { get; set; }

        public int DfltTrnsfrPackListFormKey { get; set; }

        public int DfltWhseKey { get; set; }

        public short DropShipCostMeth { get; set; }

        public int DropShipHiddenBatchKey { get; set; }

        public short DropShipLeadTime { get; set; }

        public short ExplDOMOnSO { get; set; }

        public short ExplPCOnSO { get; set; }

        public short ExtShipSystem { get; set; }

        public int ExtShipSystemWeightUOMKey { get; set; }

        public short GLPostRgstrFormat { get; set; }

        public short GrossProfitCost { get; set; }

        public short IntegrateWithGL { get; set; }

        public short IntegrateWithIM { get; set; }

        public short IntegrateWithPO { get; set; }

        public DateTime? LastSalesHistPurge { get; set; }

        public DateTime? LastSalesOrdPurge { get; set; }

        public DateTime? LastShipPurge { get; set; }

        public short MaintAuditRetnt { get; set; }

        public short MaxSerialNbrsOnPickList { get; set; }

        public short OpenOrdCreditChk { get; set; }

        public short OverShipmentPolicy { get; set; }

        public short PerformValidationDuringSave { get; set; }

        public short PostInDetlDropShip { get; set; }

        public short PostInDetlSalesClr { get; set; }

        public short QuoteRetnt { get; set; }

        public short RecentCustItemDays { get; set; }

        public short RecentRtrnDays { get; set; }

        public short RecentShipDays { get; set; }

        public short ReqCompletePacking { get; set; }

        public short ReqLotSerialDistInPacking { get; set; }

        public short ReqSalesSourceOnSO { get; set; }

        public int RestckSTaxClassKey { get; set; }

        public int ReStockAcctKey { get; set; }

        public short ReverseCommOnRtrn { get; set; }

        public short ReverseFreightOnRtrn { get; set; }

        public short RMAExpirationDays { get; set; }

        public short RMAReqd { get; set; }

        public decimal RMAThresholdAmt { get; set; }

        public int SalesClearAcctKey { get; set; }

        public short SalesHistRetnt { get; set; }

        public short SalesOrderReqdForRtrn { get; set; }

        public short SalesOrdRetnt { get; set; }

        public short SameRangeForBlnkt { get; set; }

        public short SameRangeForQuote { get; set; }

        public short ShipDeclaredAmtCostMeth { get; set; }

        public decimal ShipDeclaredAmtFixedAmt { get; set; }

        public decimal ShipDeclaredAmtMarkupAmt { get; set; }

        public decimal ShipDeclaredAmtMarkupPct { get; set; }

        public short ShipDeclaredAmtMeth { get; set; }

        public int ShipmentHiddenBatchKey { get; set; }

        public short ShipRetnt { get; set; }

        public short SingleUserSO { get; set; }

        public short SOAuditAdd { get; set; }

        public short SOAuditChange { get; set; }

        public short SOAuditDelete { get; set; }

        public int SOPaymentBatchKey { get; set; }

        public int SystemBatchKey { get; set; }

        public short TrackChngOrders { get; set; }

        public short TrackDelSO { get; set; }

        public string TranIDChngChar { get; set; }

        public string TranIDRelChar { get; set; }

        public int UpdateCounter { get; set; }

        public short UseBlnktRelNos { get; set; }

        public short UseMultCurr { get; set; }

        public short ValidLotReqdForRtrn { get; set; }

        public short ValidSerialReqdForRtrn { get; set; }

        public short WarnForGrossProfit { get; set; }


    }
}