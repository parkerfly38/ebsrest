using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class InsertAutoShipQRequest
    {
        public int SOKey { get; set; }

        public int? SOLineKey { get; set; }

        /// <summary>
        /// Begin Point (1 After SO Create (default), 2 After Create Picks, 3 after confirm picks), optional
        /// </summary>
        public int? BeginPoint { get; set; } = 1;

        /// <summary>
        /// 1 after shipments are generated (default), after create picks, after confirm picks), optional
        /// </summary>
        public int? EndPoint { get; set; } = 1;

        [MaxLength(3)]
        public string CompanyID { get; set; }

        public string LoginName { get; set; }
    }
}