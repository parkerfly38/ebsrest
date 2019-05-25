using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Data.Common;
using Dapper;

namespace ebsrest
{
    public class ConnectionFactory
    {
        public static Dictionary<string, string> ConnectionStrings = new Dictionary<string, string>();

        public static DbConnection GetOpenConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
        
    }
}