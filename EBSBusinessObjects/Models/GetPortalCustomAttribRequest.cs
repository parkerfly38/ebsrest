using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetPortalCustomAttribRequest
    {
        [Required]
        [MaxLength(3)]
        public string CompID { get; set; }

        [MaxLength(50)]
        public string AttribName { get; set; }

        public string LoginName { get; set; }
    }
}