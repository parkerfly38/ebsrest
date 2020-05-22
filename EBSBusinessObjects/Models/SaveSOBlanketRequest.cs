using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class SaveSOBlanketRequest
    {
        [Required]
        public int SOKey { get; set; }

        [Required]
        public int SOKeyTemp { get; set; }

        public string LoginName { get; set; }
    }
}