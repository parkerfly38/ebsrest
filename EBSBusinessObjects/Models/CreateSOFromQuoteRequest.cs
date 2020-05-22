using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class CreateSOFromQuoteRequest
    {
        [MaxLength(3)]
        public string CompanyID { get; set; }

        public int? SOKeyIn { get; set; }

        public DateTime? TranDate { get; set; }

        [MaxLength(10)]
        public string TranNo { get; set; }

        [MaxLength(30)]
        public string User { get; set; }

        public int SessionID { get; set; }

        public string LoginName { get; set; }
    }
}