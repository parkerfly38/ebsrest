using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class AutoShipResults
    {
        public AutoShipResults()
        {
            Batch = new List<AutoShipTransBatch>();
        }

        List<AutoShipTransBatch> Batch { get; set; }
    }

    public class AutoShipTransBatch
    {
        public int RowKey { get; set; }

        public string ARBatchCmnt { get; set; }

        public int? BeginPoint { get; set; }

        public int? EndPoint { get; set; }

        public DateTime? PostDate { get; set; }

        public string OvrdSegValue { get; set; }

        public string RefBatchID { get; set; }

        public string WhseID { get; set; }

        public string ErrorMsg { get; set; }

        public int ProcessStatus { get; set; }

        public int RetARBatchKey { get; set; }

        public int RetARBatchNo { get; set; }
    }

    public class AutoShipShipments
    {
        public int RowKey { get; set; }

        public string RefBatchID { get; set; }

        public string RefShipmentID { get; set; }

        public string ShipMethID { get; set; }

        public string SONumber { get; set; }

        public DateTime TranDate { get; set; }

        public string ErrorMsg { get; set; }

        public int ProcessStatus { get; set; }
    }
}