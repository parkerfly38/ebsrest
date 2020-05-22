using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class SOAddr
    {
        public int SOAddrKeyTemp { get; set; }

        public int SOKeyTemp { get; set; }

        public int AddrKey { get; set; }

        public string CustAddrID { get; set; }

        public string ShipToAddrName { get; set; }

        public string ShipToAddrLine1 { get; set; }

        public string ShipToAddrLine2 { get; set; }

        public string ShipToAddrLine3 { get; set; }

        public string ShipToAddrLine4 { get; set; }

        public string ShipToAddrLine5 { get; set; }

        public string ShipToCity { get; set; }

        public string ShipToStateID { get; set; }

        public string ShipToPostalCode { get; set; }

        public string ShipToCountryID { get; set; }

        /// <summary>
        /// request only, (S)ave for (D)elete
        /// </summary>
        [MaxLength(1)]
        public string SorD { get; set; }

        /// <summary>
        /// Request only, system login name
        /// </summary>
        public string LoginName { get; set; }
    }
}