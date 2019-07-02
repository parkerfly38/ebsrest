using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class CheckSOLogRequest
    {
        [MaxLength(3)]
        public string CompID { get; set; }

        public int? TranType { get; set; }

        [MaxLength(10)]
        public string TranNo { get; set; }

        public string LoginName { get; set; }
    }
}