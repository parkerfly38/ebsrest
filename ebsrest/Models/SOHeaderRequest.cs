using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class SOHeaderRequest
    {
        [Required]
        [MaxLength(3)]
        public string CompID { get; set; }

        public int? SOKeyTemp { get; set; }

        [MaxLength(50)]
        public string PortalSessionID { get; set; }

        [MaxLength(30)]
        public string SearchLoginName { get; set; }

        public int? Status { get; set; }

        [MaxLength(12)]
        public string CustID { get; set; }

        [MaxLength(15)]
        public string CustPONo { get; set; }

        [MaxLength(100)]
        public string ImportRef { get; set; }

        [MaxLength(30)]
        public string LoginName { get; set; }
    }
}