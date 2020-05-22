using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class SaveSOLineUDFRequest
    {
        /// <summary>
        /// 0 for new, line value to save.
        /// </summary>
        [Required]
        public int SOLineKeyTemp { get; set; }

        /// <summary>
        /// 0 for new, line value to save.
        /// </summary>
        [Required]
        public int FieldKey { get; set; }

        [Required]
        [MaxLength(255)]
        public string FieldValue { get; set; }

        /// <summary>
        /// 1 to delete a line UDF value.
        /// </summary>
        public int? Delete { get; set; }

        public string LoginName { get; set; }
    }
}