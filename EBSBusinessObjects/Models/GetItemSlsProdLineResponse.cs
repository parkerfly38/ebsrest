using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetItemSlsProdLineResponse
    {
        public string CompanyID { get; set; }
        public int SalesProdLineKey { get; set; }
        public string SalesProdLineID { get; set; }
        public string Description { get; set; }
    }
}