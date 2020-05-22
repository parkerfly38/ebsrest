using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class ProcessAutoshipFromQRequest
    {
        public int AutoShipQID { get; set; }

        public int LoadStg { get; set; }

        public string LoginName { get; set; }
    }
}