using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetOrderDatesRequest
    {
        [Required]
        [MaxLength(3)]
        public string CompID { get; set; }

        [Required]
        [MaxLength(12)]
        public string CustID { get; set; }

        public string CustShipID { get; set; }

        public DateTime? HdrReqDate { get; set; }

        public DateTime? LineReqDate { get; set; }

        public string LoginName { get; set; }
    }
}