using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class CalcCartSalesTaxRequest
    {
        public int SOKeyTemp { get; set; }

        [MaxLength(50)]
        public string LoginName { get; set; }
    }
}