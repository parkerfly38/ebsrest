using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetWhseBinsRequest
    {
        [Required]
        [MaxLength(3)]
        public string CompID { get; set; }

        public int WhseKey { get; set; }

        public string WhseBinID { get; set; }

        public string LoginName { get; set; }

    }
}