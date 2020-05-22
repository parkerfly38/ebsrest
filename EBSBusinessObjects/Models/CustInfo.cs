using System;
using System.Collections.Generic;
using System.Text;

namespace EBSBusinessObjects.Models
{
    public class CustInfo
    {
        public string CompanyID { get; set; }
        public int CustKey { get; set; }
        public string CustID { get; set; }
        public string CustName { get; set; }
        public string CustRefNo { get; set; }
        public string DateEstab { get; set; }
        public int Hold { get; set; }
        public string BillAddrName { get; set; }
        public string BillAddrLine1 { get; set; }
        public string BillAddrLine2 { get; set; }
        public string BillAddrLine3 { get; set; }
        public string BillAddrLine4 { get; set; }
        public string BillAddrLine5 { get; set; }
        public string BillAddrCity { get; set; }
        public string BillAddrState { get; set; }
        public string BillAddrPostalCode { get; set; }
        public string BillAddrCountry { get; set; }
        public string BillAddrPhone { get; set; }
        public string BillAddrPhoneExt { get; set; }
        public string BillAddrFax { get; set; }
        public string BillAddrFaxExt { get; set; }
        public int AllowCustRefund { get; set; }
        public int AllowInvtSubst { get; set; }
        public int AllowWriteOff { get; set;}

    }
}
