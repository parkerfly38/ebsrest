using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class ShipmentScheduleRequest
    {
        [Required]
        [MaxLength(2)]
        public string Show_A_S_NS { get; set; }

        [Required]
        [MaxLength(6)]
        public string WhseID { get; set; }

        [Required]
        public DateTime Thru { get; set; }
        
        [Required]
        public DateTime From { get; set; }

        public string LoginName { get; set; }

    }
}