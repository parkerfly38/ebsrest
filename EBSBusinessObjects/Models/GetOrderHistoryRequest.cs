using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetOrderHistoryRequest
    {
        [Required]
        [MaxLength(3)]
        public string CompID { get; set; }

        [Required]
        public int ItemKey { get; set; }

        [Required]
        public int CustKey { get; set; }

        public string LoginName { get; set; }
    }
}