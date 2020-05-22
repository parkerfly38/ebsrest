using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class SOLine
    {
        public int SOLineKeyTemp { get; set; }

        public int SOKeyTemp { get; set; }

        public string ItemID { get; set; }

        public decimal Price { get; set; }

        public int PriceOverride { get; set; }

        public decimal Quantity { get; set; }

        public string Description { get; set; }

        public int POLineNo { get; set; }

        public string UnitMeasID { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime? ExpDate { get; set; }

        public DateTime ShipDate { get; set; }

        public DateTime? PromiseDate { get; set; }

        public string TaxClassID { get; set; }

        public int TaxClassKey { get; set; }

        public string UserFld1 { get; set; }

        public string UserFld2 { get; set; }

        public string CustItem { get; set; }

        public int UMKey { get; set; }

        public decimal ExtdAmt { get; set; }

        public int ItemKey { get; set; }

        public string Notes { get; set; }

        public int BackOrder { get; set; }

        public decimal TaxAmt { get; set; }

        public decimal UnitCost { get; set; }

        public decimal GMAmt { get; set; }

        public decimal GMPct { get; set; }

        public int AllowDecimalQty { get; set; }

        public decimal MinSaleQty { get; set; }

        public decimal SaleMutiple { get; set; }

        public string SalesPromoID { get; set; }

        public int SalesPromoKey { get; set; }

        public int AcctRefKey { get; set; }

        public decimal StdPrice { get; set; }

        public int WhseKey { get; set; }

        public decimal FreightAmt { get; set; }

        public decimal TradeDiscAmt { get; set; }

        public int ePortalPromoKey { get; set; }

        public string LineShipStatus { get; set; }

        public int ShipMethKey { get; set; }

        public string ShipMethIDLn { get; set; }

        public string isDropShip { get; set; }

        public string QtyAvail { get; set; }

        public string TradeDisc { get; set; }

        /// <summary>
        /// input only, save or delete line
        /// </summary>
        [MaxLength(1)]
        public string SorD { get; set; }

        public string LoginName { get; set; }
    }
}