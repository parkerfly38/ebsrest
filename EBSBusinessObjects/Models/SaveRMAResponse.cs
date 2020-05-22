using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class SaveRMAResponse
    {
        public RMADetails rmaDetails { get; set; }

        public List<RMALineDetails> rmaLineDetails { get; set; }
    }

    public class RMADetails
    {
        public string Result { get; set; }

        public string RMANbr { get; set; }

        public int RMAKey { get; set; }
    }

    public class RMALineDetails
    {
        public string Result { get; set; }

        public int RMALineKey { get; set; }
    }


}