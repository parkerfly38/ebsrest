using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ebsrest
{
    public class SqlHandler
    {


        public void SQLExecuteWithoutReturn(string commandText, CommandType commandType, DynamicParameters parameterCollection)
        {
            
            using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
            {
                connection.Execute(commandText, parameterCollection, commandType: commandType);
            }
        }

        public T SQLWithRetrieveSingle<T>(string commandText, CommandType commandType, DynamicParameters parameterCollection)
        {
            T returnObj;
            using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
            {
                returnObj = connection.Query<T>(commandText, parameterCollection, commandType: commandType).FirstOrDefault();
            }

            return returnObj;
        }

        public List<T> SQLWithRetrieveList<T>(string commandText, CommandType commandType, DynamicParameters parameterCollection)
        {
            List<T> returnObj;
            using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
            {
                returnObj = connection.Query<T>(commandText, parameterCollection, commandType: commandType).ToList();
            }

            return returnObj;
        }
    }
}