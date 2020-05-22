using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class SaveSalesOrderUDFRequest
    {
        /// <summary>
        /// Zero or null to add new UDF field value
        /// </summary>
        public int? SOKeyTemp { get; set; }

        /// <summary>
        /// Zero or null in tandem with SOKeyTemp for new UDF Field value
        /// </summary>
        public int? FieldKey { get; set; }

        [MaxLength(255)]
        [Required]
        public string FieldValue { get; set; }

        /// <summary>
        /// 1 to delete.
        /// </summary>
        public int? Delete { get; set; }

        public string LoginName { get; set; }
    }
}