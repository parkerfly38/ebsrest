using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace EBSBusinessObjects.Models
{
    public class SearchRMARequest
    {
        [Required]
        [MaxLength(3)]
        public string CompID { get; set; }

        public int? RMAKey { get; set; }

        [MaxLength(10)]
        public string TranNo { get; set; }

        [MaxLength(13)]
        public string TranID { get; set; }

        [MaxLength(12)]
        public string CustID { get; set; }

        public int? CustKey { get; set; }

        [MaxLength(10)]
        public string CreateDateFr { get; set; }

        [MaxLength(10)]
        public string CreateDateTo { get; set; }

        [MaxLength(10)]
        public string ExpDateFr { get; set; }

        [MaxLength(10)]
        public string ExpDateTo { get; set; }

        [MaxLength(12)]
        public string SperID { get; set; }

        [MaxLength(200)]
        public string SperKeyIn { get; set; }

        public string LoginName { get; set; }
    }
}