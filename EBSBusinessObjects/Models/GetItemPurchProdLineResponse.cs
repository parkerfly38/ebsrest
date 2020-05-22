using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetItemPurchProdLineResponse
    {
        public int PurchProdLineKey { get; set; }

        public int AllowVendBackOrd { get; set; }

        public string Description { get; set; }

        public string PurchProdLineID { get; set; }

        public int BuyerKey { get; set; }

        public int ItemType { get; set; }

        public string BuyerID { get; set; }
    }
}