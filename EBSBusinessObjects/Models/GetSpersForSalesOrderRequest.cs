using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetSpersForSalesOrderRequest
    {
        [Required]
        [MaxLength(3)]
        public string CompanyID { get; set; }

        [MaxLength(12)]
        public string SperID { get; set; }

        public int? SperKey { get; set; }

        [MaxLength(500)]
        public string SperKeyIn { get; set; }

        public string LoginName { get; set; }
    }
}