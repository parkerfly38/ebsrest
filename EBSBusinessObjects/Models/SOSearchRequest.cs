using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class SOSearchRequest
    {
        [Required]
        [MaxLength(3)]
        public string CompID { get; set; }

        [MaxLength(15)]
        public string CustPONo { get; set; }

        [MaxLength(12)]
        public string CustID { get; set; }

        [MaxLength(1000)]
        public string SperKeyIn { get; set; }

        public string FromTranDate { get; set; }

        public string ToTranDate { get; set; }

        [MaxLength(1000)]
        public string SalesProdLineKeyIn { get; set; }

        [MaxLength(1000)]
        public string ItemClassKeyIn { get; set; }

        [MaxLength(1000)]
        public string CustKeyIn { get; set; }

        [MaxLength(12)]
        public string VendID { get; set; }

        [MaxLength(30)]
        public string ItemID { get; set; }

        public int TranType { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        [MaxLength(30)]
        public string LoginName { get; set; }
    }
}