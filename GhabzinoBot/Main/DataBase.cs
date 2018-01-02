using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Ayantech.WebService
{
    public static partial class DataBase
    {
        public static QueryResult GetDatabaseInformation(string keyName)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_getDatabaseInformation";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@keyName", Value = keyName });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult SetTelegramUserInfo(long telegramId, string mobileNumber, string token, int userState, int userField, int waterBillInquiryStep, int gasBillInquiryStep, int electricityBillInquiryStep, int mciMobileBillInquiryStep, int fixedLineInquiry, int trafficFinesInquiry, int payBill, int history)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_setTelegramUserInfo";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@telegramID", Value = telegramId });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@mobileNumber", Value = mobileNumber });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.VarChar, ParameterName = "@token", Value = token });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@userState", Value = userState });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@userField", Value = userField });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@waterBillInquiryStep", Value = waterBillInquiryStep });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@gasBillInquiryStep", Value = gasBillInquiryStep });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@electricityBillInquiryStep", Value = electricityBillInquiryStep });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@mciMobileBillInquiryStep", Value = mciMobileBillInquiryStep });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@fixedLineInquiry", Value = fixedLineInquiry });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@trafficFinesInquiry", Value = trafficFinesInquiry });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@payBill", Value = payBill });
            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.Int, ParameterName = "@history", Value = history });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
        public static QueryResult GetTelegramUserInfo(long telegramId)
        {
            var sqlParameters = new List<SqlParameter>();
            var storedProcedureName = "ud_prc_getTelegramUserInfo";

            sqlParameters.Add(new SqlParameter { SqlDbType = SqlDbType.BigInt, ParameterName = "@telegramID", Value = telegramId });
            return ExecuteStoredProcedure(storedProcedureName, sqlParameters);
        }
    }
}