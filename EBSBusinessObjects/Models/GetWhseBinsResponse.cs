using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetWhseBinsResponse
    {
        public string Location { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public string WhseBinId { get; set; }
        public int WhseBinKey { get; set; }
        public int WhseKey { get; set; }
        public string WhseID { get; set; }
        public string WHDescription { get; set; }
        public int WhseZoneKey { get; set; }
        public string WhseZoneID { get; set; }
        public int DfltBin { get; set; }
        public int SortOrder { get; set; }
        public int TempBin { get; set; }
        public string CompanyID { get; set; }
        public string TypeDesc { get; set; }
    }
}