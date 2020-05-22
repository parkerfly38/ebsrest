using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class CreateSOFromQuoteOrBlanketResponse
    {
        public int RetVal { get; set; }

        public int SOKey { get; set; }

        public string TranID { get; set; }
    }
}