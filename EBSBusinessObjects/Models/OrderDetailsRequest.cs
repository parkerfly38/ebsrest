using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class OrderDetailsRequest
    {
        [MaxLength(3)]
        public string compid { get; set; }

        public int SOKey { get; set; }

        public string LoginName { get; set; }
    }
}