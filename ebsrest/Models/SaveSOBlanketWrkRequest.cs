using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class SaveSOBlanketWrkRequest
    {
        public int? SOKeyTemp { get; set; }

        public decimal? MaxAmountToGen { get; set; }

        public int MaxSOToGen { get; set; }

        public int? Contract { get; set; }

        public string ContractRef { get; set; }

        public int? CurrExchRateMeth { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime StopDate { get; set; }

        [Required]
        public int ProcCycleKey { get; set; }

        [Required]
        [MaxLength(1)]
        public string SorD { get; set; }

        public string LoginName { get; set; }
    }
}