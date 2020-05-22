using Dapper;
using ebsrest.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using EBSBusinessObjects.Models;

namespace ebsrest
{
    /// <summary>
    /// Library of Common Functions
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// Returns custom portal attributres
        /// </summary>
        /// <param name="compID"></param>
        /// <param name="attribName"></param>
        /// <param name="loginName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns ship method id for company/customer combination
        /// </summary>
        /// <param name="compId"></param>
        /// <param name="CustID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get ship method by address, company, and customer
        /// </summary>
        /// <param name="compId"></param>
        /// <param name="custID"></param>
        /// <param name="Addr1"></param>
        /// <param name="Addr2"></param>
        /// <param name="Addr3"></param>
        /// <param name="Addr4"></param>
        /// <param name="Addr5"></param>
        /// <param name="Name"></param>
        /// <param name="City"></param>
        /// <param name="Country"></param>
        /// <param name="State"></param>
        /// <param name="Postal"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets an ItemKey by ItemID and Company ID
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="compId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets Vendor Address Key by company, vendor, and addr id
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="vendorId"></param>
        /// <param name="vendorAddrId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get Vendor Key by Vendor ID
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="vendorId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets an item description by item ID
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Primary error logging function
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="shortMessage"></param>
        /// <param name="longMessage"></param>
        /// <param name="webPage"></param>
        /// <param name="IWE"></param>
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

        /// <summary>
        /// Validates Customer ID
        /// </summary>
        /// <param name="custID"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets a Pmt Terms Key by Pmt Terms ID
        /// </summary>
        /// <param name="pmtTermsID"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Create a SessionKey
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Primary Item Pricing routine
        /// </summary>
        /// <param name="CompID"></param>
        /// <param name="CustID"></param>
        /// <param name="ItemID"></param>
        /// <param name="AddrKey"></param>
        /// <param name="UOMKey"></param>
        /// <param name="Qty"></param>
        /// <param name="LoginName"></param>
        /// <param name="PriceInquiry"></param>
        /// <param name="WhseKey"></param>
        /// <returns></returns>
        public static ItemPrice GetItemPrice(string CompID, string CustID, string ItemID, int? AddrKey, int? UOMKey, decimal? Qty, string LoginName, int PriceInquiry, int? WhseKey)
        {
            var itemPrice = new ItemPrice();



            return itemPrice;
        }

    }
}