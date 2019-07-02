using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class GetRecurBlnktOrdersRequest
    {
        [MaxLength(3)]
        public string CompID { get; set; }

        public int? WhseKey { get; set; }

        [MaxLength(13)]
        public string TranID { get; set; }

        public int? Priority { get; set; }

        [MaxLength(15)]
        public string CustPONo { get; set; }

        public int? ItemKey { get; set; }

        public int? CustKey { get; set; }

        public int? ItemType { get; set; }

        [Required]
        public int IncludeDetails { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        [MaxLength(2)]
        public string BlnktType { get; set; }

        [MaxLength(1000)]
        public string CustKeys { get; set; }

        public string LoginName { get; set; }
    }
}