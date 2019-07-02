using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class SOBlanket
    {
        public int Contract { get; set; }

        public string ContractRef { get; set; }

        public int CurrExchRateMeth { get; set; }

        public decimal MaxAmountToGen { get; set; }

        public int MaxSOToGen { get; set; }

        public int SOKeyTemp { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? StopDate { get; set; }

        public int ProcCycleKey { get; set; }
    }
}