using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class OrderError
    {
        public string ColumnID { get; set; }

        public string ColumnValue { get; set; }

        public string Comment { get; set; }

        public string Duplicate { get; set; }

        public string EntityID { get; set; }

        public string EntryNo { get; set; }

        public string SessionKey { get; set; }

        public string Status { get; set; }
    }
}