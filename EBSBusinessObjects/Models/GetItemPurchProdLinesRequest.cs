using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetItemPurchProdLinesRequest
    {
        [MaxLength(3)]
        [Required]
        public string CompID { get; set; }

        public string PurchProdLineID { get; set; }

        [MaxLength(50)]
        public string LoginName { get; set; }
    }
}