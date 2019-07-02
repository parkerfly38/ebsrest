using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class SOAttr
    {
        [Required]
        [MaxLength(1)]
        public string AttribType { get; set; }

        [Required]
        public int AttribRefKey { get; set; }

        [MaxLength(50)]
        public string AttribCharValue { get; set; }

        public decimal? AttribDecValue { get; set; }

        public int? AttribIntValue { get; set; }

        [Required]
        public int AttribKey { get; set; }

        [Required]
        [MaxLength(1)]
        public string SorD { get; set; }

        public string LoginName { get; set; }
    }
}