using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class SearchShipmentsRequest
    {
        public string LoginName { get; set; }
        [Required]
        [MaxLength(3)]
        public string CompID { get; set; }

        [MaxLength(12)]
        public string CustID { get; set; }

        [MaxLength(13)]
        public string TranID { get; set; }

        [MaxLength(13)]
        public string SalesOrderID { get; set; }

        [MaxLength(15)]
        public string CustPONo { get; set; }

        [MaxLength(1)]
        public string CustPOBCE { get; set; }

        [MaxLength(15)]
        public string CustPONoFr { get; set; }

        [MaxLength(15)]
        public string CustPONoTo { get; set; }

        public DateTime? FromTranDate { get; set; }

        public DateTime? ToTranDate { get; set; }

        public string SperKeyIn { get; set; }

        public string CustKeyIn { get; set; }

        public int? AddrKey { get; set; }

        public int? ShipMethKey { get; set; }

        public string ShipTrackNo { get; set; }

        public string InvcTranID { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}