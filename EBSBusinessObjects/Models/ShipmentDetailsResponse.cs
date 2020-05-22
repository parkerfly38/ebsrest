using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class ShipmentDetailsResponse
    {
        public ShipmentDetailsHeader header { get; set; }

        public List<ShipmentDetailsLine> lines { get; set; }
    }

    public class ShipmentDetailsHeader
    {
        public decimal AmtShipped { get; set; }

        public string BillOfLading { get; set; }

        public string CompanyID { get; set; }

        public DateTime? CreateDate { get; set; }

        public short CreateType { get; set; }

        public string TranType { get; set; }

        public string CreateTypeAsText { get; set; }

        public string CreateUserID { get; set; }

        public string CustID { get; set; }

        public string CustName { get; set; }

        public decimal FreightAmt { get; set; }

        public DateTime PostDate { get; set; }

        public string RcvgWhseDesc { get; set; }

        public string RcvgWheseID { get; set; }

        public short ShipLabelsPrinted { get; set; }

        public string ShipMethDesc { get; set; }

        public string ShipMethID { get; set; }

        public string ShipToCustAddrID { get; set; }

        public string ShipToAddrLine1 { get; set; }

        public string ShipToAddrLine2 { get; set; }

        public string ShipToAddrLine3 { get; set; }

        public string ShipToAddrLine4 { get; set; }

        public string ShipToAddrLine5 { get; set; }

        public string ShipToAddrName { get; set; }

        public string ShipToAddrCity { get; set; }

        public string ShipToAddrCountry { get; set; }

        public string ShipToAddrPostalCode { get; set; }

        public string ShipToAddrState { get; set; }

        public string ShipTransfrID { get; set; }

        public string ShipWhseDesc { get; set; }

        public string ShipWhseID { get; set; }

        public string TrailerNo { get; set; }

        public string TranCmnt { get; set; }

        public string TranDate { get; set; }

        public string TransitPoint { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string UpdateUserId { get; set; }

        public string UserFld1 { get; set; }

        public string UserFld2 { get; set; }

        public string UserFld3 { get; set; }

        public string UserFld4 { get; set; }

        public int ShipKey { get; set; }

        public int CustKey { get; set; }

        public int RcvgWhseKey { get; set; }

        public int ShipToCustAddrKey { get; set; }

        public string TranID { get; set; }

        public short TranStatus { get; set; }

        public short PackListPrinted { get; set; }

    }

    public class ShipmentDetailsLine
    {
        public int PickListLineNo { get; set; }

        public string ItemID { get; set; }

        public string ItemShortDesc { get; set; }

        public decimal QtyShipped { get; set; }

        public string ShipUnitMeasID { get; set; }
    }
}