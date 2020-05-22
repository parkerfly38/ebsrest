using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ebsrest.Models
{
    public class UserData
    {
        public int ExtUserKey { get; set; }
        public string LoginName { get; set; }
        public string CompanyID { get; set; }
        public string ResetPassword { get; set; }
        public string Locked { get; set; }
        public DateTime? LockResetDate { get; set; }
        public DateTime? PasswordSetDate { get; set; }
        public bool Valid { get; set; }
    }
}