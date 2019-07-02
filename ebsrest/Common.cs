using Dapper;
using ebsrest.Models;
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
        public static List<PortalCustomAttributes> GetPortalCustomAttributes(string compID, string attribName, string loginName)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", compID);
            parameters.Add("@AttribName", attribName);

            List<PortalCustomAttributes> attributes = new List<PortalCustomAttributes>();

            try
            {
                using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                {
                    attributes = connection.Query<PortalCustomAttributes>("spciGetPortalCustomizeAttrib_rkl", parameters, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception exception)
            {
                LogError(loginName, exception.Message, exception.StackTrace, "Common.GetPortalCustomAttributes", "E");
            }
            // we have deliberately disabled licensing at this point;
            return attributes;
        }

        public static string GetShipMethID(string compId, string CustID)
        {
            string ShipMethID = string.Empty;

            SqlHandler sqlHandler = new SqlHandler();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", compId);
            parameters.Add("@CustID", CustID);

            ShipMethID = sqlHandler.SQLWithRetrieveSingle<string>("sparGetCustShipMethID_rkl", System.Data.CommandType.StoredProcedure, parameters);

            return ShipMethID;
        }

        public static string GetShipMethID(string compId, string custID, string Addr1, string Addr2, string Addr3, string Addr4, string Addr5, string Name, string City, string Country, string State, string Postal)
        {
            string ShipMethID = string.Empty;

            SqlHandler sqlHandler = new SqlHandler();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompanyID", compId);
            parameters.Add("@CustID", custID);
            parameters.Add("@Addr1", Addr1);
            parameters.Add("@Addr2", Addr2);
            parameters.Add("@Addr3", Addr3);
            parameters.Add("@Addr4", Addr4);
            parameters.Add("@Addr5", Addr5);
            parameters.Add("@Name", Name);
            parameters.Add("@City", City);
            parameters.Add("@CountryID", Country);
            parameters.Add("@StateID", State);
            parameters.Add("@PostalCode", Postal);

            ShipMethID = sqlHandler.SQLWithRetrieveSingle<int>("sparGetCustAddrShipMethID_RKL", System.Data.CommandType.StoredProcedure, parameters).ToString();

            return ShipMethID;
        }

        public static int GetItemKey(string itemId, string compId)
        {
            int ItemKey = 0;

            SqlHandler sqlHandler = new SqlHandler();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ItemID", itemId);
            parameters.Add("@CompID", compId);
            dynamic returnObj = sqlHandler.SQLWithRetrieveSingle<dynamic>("spimGetItemInfo_RKL", System.Data.CommandType.StoredProcedure, parameters);
            if (returnObj != null)
                ItemKey = returnObj.ItemKey;
            return ItemKey;
        }

        public static int GetVendorAddressKey(string companyId, string vendorId, string vendorAddrId)
        {
            int VendAddrKey = 0;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@compid", companyId);
            parameters.Add("@vendid", vendorId);
            parameters.Add("@VendaddrID", vendorAddrId);
            SqlHandler sqlHandler = new SqlHandler();

            List<dynamic> returnObj = sqlHandler.SQLWithRetrieveSingle<dynamic>("spapGetVendorAddresses_RKL", System.Data.CommandType.StoredProcedure, parameters).ToList();
            if (returnObj.Count > 0)
            {
                VendAddrKey = returnObj[0].AddrKey;
            }
            return VendAddrKey;
        }

        public static int GetVendorKey(string companyId, string vendorId)
        {
            int VendKey = 0;
            SqlHandler sqlHandler = new SqlHandler();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@compid", companyId);
            parameters.Add("@vendid", vendorId);

            dynamic returnObj = sqlHandler.SQLWithRetrieveSingle<dynamic>("spapGetVendorInfo_RKL", System.Data.CommandType.StoredProcedure, parameters);
            VendKey = returnObj.VendKey;

            return VendKey;
        }

        public static string GetItemDesc(string companyID, string itemID)
        {
            string returnString = string.Empty;
            SqlHandler sqlHandler = new SqlHandler();
            
            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@CompID", companyID);
            sqlParameters.Add("@ItemID", itemID);

            dynamic returnObj = sqlHandler.SQLWithRetrieveSingle<dynamic>("spimGetItemInfo_RKL", System.Data.CommandType.StoredProcedure, sqlParameters).FirstOrDefault();
            returnString = returnObj.ShortDesc;

            return returnString;
        }

        public static void LogError(string loginName, string shortMessage, string longMessage, string webPage, string IWE)
        {
            if (!longMessage.Contains("ThreadAbortException"))
            {
                SqlHandler sqlHandler = new SqlHandler();
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@LoginName", loginName);
                    parameters.Add("@ShortMessage", shortMessage.Replace("'", "''"));
                    parameters.Add("@LongMessage", longMessage.Replace("'", "''"));
                    parameters.Add("@WebPage", webPage);
                    parameters.Add("@IWE", IWE);

                    sqlHandler.SQLExecuteWithoutReturn("spciInsertWebLog_rkl", System.Data.CommandType.StoredProcedure, parameters);
                }
                catch (Exception exception)
                {
                    //do nothing because db error
                }
            }
        }

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