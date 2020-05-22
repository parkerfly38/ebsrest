using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetAutoShipQDetailsResponse
    {
        public int Row { get; set; }

        public int AutoShipQID { get; set; }

        public int SOKey { get; set; }

        public int SOLineKey { get; set; }

        public int BeginPoint { get; set; }

        public int EndPoint { get; set; }

        public string LoginName { get; set; }

        public string CompanyId { get; set; }

        public string Status { get; set; }

        public int SessionKey { get; set; }

        public string EnQDt { get; set; }

        public string ProcessDT { get; set; }

        public string Message { get; set; }

        public string SOTranID { get; set; }

        public string SOTranNo { get; set; }
    }
}