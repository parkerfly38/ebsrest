using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EBSBusinessObjects.Models
{
    public class GetAutoShipResultsResponse
    {
        public List<AutoShipBatch> Batch { get; set; }
        public List<AutoShipShipment> Shipments { get; set; }
        public List<AutoShipShipmentLine> ShipmentLines { get; set; }
        public List<AutoShipShipmentDist> ShipmentDist { get; set; }
        public List<AutoShipmentQueueError> ASErrors { get; set; }
        public List<AutoShipQueueLog> QLog { get; set; }
        public List<AutoShipTransactions> Transactions { get; set; }
    }

    public class AutoShipBatch
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

    public class AutoShipShipment
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

    public class AutoShipShipmentLine
    {
        public int RowKey { get; set; }
        public decimal QtyShip { get; set; }
        public string RefShipmentID { get; set; }
        public string SONumber { get; set; }
        public int SOLineNo { get; set; }
        public string ErrorMsg { get; set; }
        public int ShipLineKey { get; set; }
        public int ProcessStatus { get; set; }
    }

    public class AutoShipShipmentDist
    {
        public int RowKey { get; set; }
        public string BinID { get; set; }
        public decimal? DistQty { get; set; }
        public string InvtLotKey { get; set; }
        public int InvtSerialKey { get; set; }
        public int LotNo { get; set; }
        public string RefShipmentID { get; set; }
        public string SerialNo { get; set; }
        public string SONumber { get; set; }
        public int SOLineNo { get; set; }
        public int ShipLineKey { get; set; }
        public string ErrorMsg { get; set; }
        public int ProcessStatus { get; set; }
    }

    public class AutoShipmentQueueError
    {
        public string ColumnID { get; set; }
        public string ColumnValue { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
    }

    public class AutoShipQueueLog
    {
        public int AutoShipQID { get; set; }
        public string AutoShipStep { get; set; }
        public string ErrorMsg { get; set; }
        public string ErrorDateTime { get; set; }
        public string ErrLevel { get; set; }
        public string ErrLevelDesc { get; set; }
    }

    public class AutoShipTransactions
    {
        public string SOLineKey { get; set; }

        public string SOTranID { get; set; }

        public string SHTranID { get; set; }

        public string INTranID { get; set; }

        public string SHTranStatusDesc { get; set; }
    }
}