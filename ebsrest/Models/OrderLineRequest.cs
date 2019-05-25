using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class OrderLineRequest
    {
        [Required]
        [MaxLength(30)]
        public string ItemID { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime RequestDate { get; set; }

        public DateTime ShipDate { get; set; }

        public DateTime PromiseDate { get; set; }

        [Required]
        public int POLineNo { get; set; }

        [Required]
        [MaxLength(13)]
        public string OrderID { get; set; }

        [MaxLength(6)]
        [Required]
        public string UnitMeasID { get; set; }

        [MaxLength(15)]
        [Required]
        public string TaxClassID { get; set; }
        
        [MaxLength(15)]
        public string UserFld1 { get; set; }
        
        [MaxLength(15)]
        public string UserFld2 { get; set; }

        [Required]
        [MaxLength(1)]
        public string Backorder { get; set; }

        public int AcctRefKey { get; set; }

        public int WhseKey { get; set; }

        public decimal FreightAmt { get; set; }

        public decimal TradeDiscAmt { get; set; }

        [MaxLength(255)]
        public string ExtCmnt { get; set; }

        public int SessKey { get; set; }

        public int STaxSchdKey { get; set; }

        public int ShipMethKey { get; set; }

        [MaxLength(15)]
        public string ShipMethID { get; set; }

        public int DelMethod { get; set; }

        [MaxLength(12)]
        public string VendID { get; set; }

        [MaxLength(15)]
        public string VendAddrID { get; set; }
    }
}