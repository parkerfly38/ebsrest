using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ebsrest
{
    public static class Common
    {
        public static bool IsValidCustID(string custID, string companyID)
        {
            SqlHandler sqlHandler = new SqlHandler();
            bool isValid = false;
            try
            {
                DynamicParameters sqlParameters = new DynamicParameters();
                sqlParameters.Add("@CustID", custID);
                sqlParameters.Add("@CompID", companyID);
                sqlParameters.Add("@CustKey", 0);

                isValid = sqlHandler.SQLWithRetrieveSingle<bool>("sparGetCustInfo_RKL", System.Data.CommandType.StoredProcedure, sqlParameters);

            }
            catch (Exception exception)
            {
                //we will add logging
            }
            return isValid;
        }

        public static int GetPmtTermsKey(string pmtTermsID, string companyID)
        {
            SqlHandler sqlHandler = new SqlHandler();
            int pmtTermsKey = 0;
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CompID", companyID);
                parameters.Add("@PmtTermsID", pmtTermsID);
                parameters.Add("@PmtTermsKey", 0);

                pmtTermsKey = sqlHandler.SQLWithRetrieveSingle<int>("spciGetPmtTerms_rkl", System.Data.CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                //we will add logging
            }
            return pmtTermsKey;
        }


        public static int GetSessionKey()
        {
            int sessionKey = 0;
            //no elegant way to handle returns so we'll do it outside of our sqlHandler
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@iTableName", "tdmMigrationLogWrk");
                parameters.Add("@oNewKey", null, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
                using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                {
                    var result = connection.Execute("spGetNextSurrogateKey", parameters, commandType: System.Data.CommandType.StoredProcedure);
                    sessionKey = parameters.Get<int>("oNewKey");
                }
            }
            catch (Exception exception)
            {
                //we will add logging
            }
            return sessionKey;
        }
    }
}