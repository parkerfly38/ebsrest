using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class SaveRMARequest
    {
        [Required]
        [MaxLength(3)]
        public string CompID { get; set; }

        [Required]
        [MaxLength(3)]
        public string CustID { get; set; }

        [Required]
        [MaxLength(15)]
        public string BillToCustAddrID { get; set; }

        [Required]
        [MaxLength(15)]
        public string ShipToCustAddrID { get; set; }

        [Required]
        public int CntctKey { get; set; }

        [MaxLength(50)]
        public string TranCmnt { get; set; }

        [MaxLength(3)]
        [Required]
        public string CurrID { get; set; }

        public decimal FreightAmt { get; set; }

        public decimal RestockAmount { get; set; }

        public decimal ReturnAmt { get; set; }

        public decimal STaxAmt { get; set; }

        public int STaxTranKey { get; set; }

        public decimal TradeDiscAmt { get; set; }

        public decimal TranAmt { get; set; }

        [MaxLength(15)]
        public string UserFld1 { get; set; }

        [MaxLength(15)]
        public string UserFld2 { get; set; }

        [MaxLength(15)]
        public string UserFld3 { get; set; }

        [MaxLength(15)]
        public string UserFld4 { get; set; }

        [MaxLength(30)]
        public string CreateUserID { get; set; }

        public int RcvgWhseKey { get; set; }

        public List<RMALine> lines { get; set; }

        public List<RMASerialNbrs> serialNbrs { get; set; }

        public string LoginName { get; set; }
        
    }

    public class RMASerialNbrs
    {
        public int POLineNo { get; set; }

        public string SerialNbr { get; set; }
    }

    public class RMALine
    {
        public int POLineNo { get; set; }

        public int RMAKey { get; set; }

        [MaxLength(40)]
        public string Description { get; set; }

        public decimal ExtAmt { get; set; }

        [MaxLength(255)]
        public string ExtCmnt { get; set; }

        public decimal FreightAmt { get; set; }

        public int ItemKey { get; set; }

        public string ItemID { get; set; }

        public int OrigShipLineKey { get; set; }

        public decimal QtyAuthForRtrn { get; set; }

        public int? RcvgWhseKey { get; set; }

        public int? ReasonCodeKey { get; set; }

        public decimal RestockAmt { get; set; }

        public int RtrnType { get; set; }

        public int? ShipMethKey { get; set; }

        public int SOLineDistKey { get; set; }

        public string SOTranNo { get; set; }

        public int SOLineNo { get; set; }

        public int? STaxClassKey { get; set; }

        public int? STaxTranKey { get; set; }

        public decimal TradeDiscAmt { get; set; }

        public int UnitMeasKey { get; set; }

        public string UnitMeasID { get; set; }

        public decimal UnitPrice { get; set; }

        public int UnitPriceOvrd { get; set; }

        [MaxLength(15)]
        public string UserFld1 { get; set; }

        [MaxLength(15)]
        public string UserFld2 { get; set; }

        [MaxLength(15)]
        public string UserFld3 { get; set; }

        [MaxLength(15)]
        public string UserFld4 { get; set; }
    }
}