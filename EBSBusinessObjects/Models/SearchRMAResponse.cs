using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class SearchRMAResponse
    {
        public int RMAKey { get; set; }

        public int BillToCustAddrKey { get; set; }

        public string CloseDate { get; set; }

        public int CntctKey { get; set; }

        public string CompanyID { get; set; }

        public string CreateDate { get; set; }

        public short CreateType { get; set; }

        public string CreateUserID { get; set; }

        public decimal CurrExchRate { get; set; }

        public int CurrExchSchdKey { get; set; }

        public string CurrID { get; set; }

        public int CustClassKey { get; set; }

        public int CustKey { get; set; }

        public string ExpirationDate { get; set; }

        public decimal FreightAmt { get; set; }

        public int PrimarySperKey { get; set; }

        public short Printed { get; set; }

        public int RcvgWhseKey { get; set; }

        public decimal RestockAmt { get; set; }

        public decimal ReturnAmt { get; set; }

        public decimal ReturnAmtHC { get; set; }

        public int? ShipToAddrKey { get; set; }

        public int ShipToCustAddrKey { get; set; }

        public string Status { get; set; }

        public decimal STaxAmt { get; set; }

        public int STaxTranKey { get; set; }

        public decimal TradeDiscAmt { get; set; }

        public decimal TranAmt { get; set; }

        public decimal TranAmtHC { get; set; }

        public string TranCmnt { get; set; }

        public string TranDate { get; set; }

        public string TranID { get; set; }

        public string TranNo { get; set; }

        public int TranType { get; set; }

        public int UpdateCounter { get; set; }

        public string UpdateDate { get; set; }

        public string UpdateUseRID { get; set; }

        public string UserFld1 { get; set; }

        public string UserFld2 { get; set; }

        public string UserFld3 { get; set; }

        public string UserFld4 { get; set; }

        public string CustID { get; set; }

        public string CustName { get; set; }

        public string SperID { get; set; }

        public string SperName { get; set; }

        public string ContEmailAddr { get; set; }

        public string ContTitle { get; set; }

        public string BillToCustAddrID { get; set; }

        public string ShipToCustAddrID { get; set; }

        public string BillToAddrName { get; set; }

        public string BillToAddrLine1 { get; set; }

        public string BillToAddrLine2 { get; set; }

        public string BillToAddrLine3 { get; set; }

        public string BillToAddrLine4 { get; set; }

        public string BillToAddrLine5 { get; set; }

        public string BillToCity { get; set; }

        public string BillToStateID { get; set; }

        public string BillToPostalCode { get; set; }

        public string BillToCountryID { get; set; }

        public string BillToPhone { get; set; }

        public string BillToFax { get; set; }

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

        public string ShipToPhone { get; set; }

        public string ShipToFax { get; set; }
    }
}