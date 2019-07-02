using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class CreateSOFromBlanketRequest
    {
        [MaxLength(3)]
        public string CompanyID { get; set; }

        [Required]
        public int SOKeyIn { get; set; }

        public DateTime? TranDate { get; set; }

        public string TranNo { get; set; }

        public string User { get; set; }

        public int SessionID { get; set; }

        public int SOKeyTempIn { get; set; }

        public string LoginName { get; set; }
    }
}