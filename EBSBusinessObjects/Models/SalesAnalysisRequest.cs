using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class SalesAnalysisRequest
    {
        [MaxLength(3)]
        public string CompID { get; set; }

        [MaxLength(2000)]
        public string CustKeyIn { get; set; }

        [MaxLength(2000)]
        public string SperKeyIn { get; set; }

        [MaxLength(7)]
        public string DateFr { get; set; }

        [MaxLength(7)]
        public string DateTo { get; set; }

        public string LoginName { get; set; }
    }
}