using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EBSBusinessObjects.Models
{
    public class GetUOMKeyRequest
    {
        [Required]
        [MaxLength(3)]
        public string CompID { get; set; }

        [Required]
        [MaxLength(6)]
        public string UOMID { get; set; }

        [MaxLength(50)]
        public string LoginName { get; set; }
    }
}
