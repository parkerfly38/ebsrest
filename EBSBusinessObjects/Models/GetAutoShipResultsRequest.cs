using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetAutoShipResultsRequest
    {
        public int? SessionKey { get; set; }

        public int? AutoShipQID { get; set; }

        [MaxLength(50)]
        public string LoginName { get; set; }
    }
}