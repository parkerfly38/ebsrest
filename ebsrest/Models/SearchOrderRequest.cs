using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ebsrest.Models
{
    public class SearchOrderRequest
    {
        public string LoginName { get; set; }

        [MaxLength(3)]
        public string compid { get; set; }

        [MaxLength(40)]
        public string customerName { get; set; }

        [MaxLength(12)]
        public string custID { get; set; }

        [MaxLength(15)]
        public string CustPONo { get; set; }

        [MaxLength(1)]
        public string CustPONoBCE { get; set; }

        [MaxLength(15)]
        public string CustPONo1 { get; set; }

        [MaxLength(15)]
        public string CustPONo2 { get; set; }

        [MaxLength(12)]
        public string salespersonID { get; set; }

        [MaxLength(40)]
        public string salespersonname { get; set; }

        [MaxLength(10)]
        public string orderDateFr { get; set; }

        [MaxLength(10)]
        public string orderDateTo { get; set; }

        [MaxLength(10)]
        public string tranNo { get; set; }

        [MaxLength(13)]
        public string tranID { get; set; }

        [MaxLength(1)]
        public string includeDetails { get; set; }

        public int? headerStatus { get; set; }

        public int? lineStatus { get; set; }

        public int? backOrderOnly { get; set; }

        public string SperKeyIn { get; set; }

        public string CustKeyIn { get; set; }

        public decimal? FromAmt { get; set; }

        public decimal? ToAmt { get; set; }

        [MaxLength(3)]
        public string CurrID { get; set; }

        [MaxLength(10)]
        public string ConfirmNo { get; set; }

        public int UnshippedOnly { get; set; }

        public int TranType { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}