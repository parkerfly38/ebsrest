using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetAutoShipQDetailsRequest
    {
        /// <summary>
        /// AutoShipQID, optional
        /// </summary>
        public int? AutoShipQID { get; set; }

        /// <summary>
        /// SO Key, optional
        /// </summary>
        public int? SOKey { get; set; }

        /// <summary>
        /// SO Tran ID, up to 13 characters
        /// </summary>
        [MaxLength(3)]
        public string SOTranID { get; set; }

        public int? SOLineKey { get; set; }

        public string SearchLoginName { get; set; }

        public string SearchStatus { get; set; }

        public int? SessionKey { get; set; }

        public DateTime? EnQDtFr { get; set; }

        public DateTime? EnQDtTo { get; set; }

        public DateTime? ProcessDtFr { get; set; }

        public DateTime? ProcessDtTo { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string LoginName { get; set; }
    }
}