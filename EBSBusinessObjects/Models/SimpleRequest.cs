using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class SimpleRequest
    {
        [MaxLength(3)]
        public string CompID { get; set; }

        [MaxLength(50)]
        public string LoginName { get; set; }
    }
}