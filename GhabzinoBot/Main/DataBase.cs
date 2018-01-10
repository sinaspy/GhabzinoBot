using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace Ayantech.WebService
{
    public static class DataBase
    {
        private static void CreateLogMessage(string logLevel, double executionTime, string commandText, CommandType commandType, string result, int code, SqlParameter[] sqlParameters)
        {
            var log = true;
            if (!logLevel.Equals("Debug"))
                log = true;
            else
                log = !ProjectValues.StoredProceduresBlockList.Contains(commandText);

            if (log)
            {
                var cp = $"(Command Type: {commandType})";
                var ct = $"(Command Text: {commandText})";
                var cd = $"(Code: {code})";
                var rs = $"(Result: {result})";
                var pr = string.Empty;

                for (int i = sqlParameters.Length - 4; i >= 0; i--)
                    if (sqlParameters[i] != null)
                    {
                        var name = sqlParameters[i].ParameterName;
                        var value = sqlParameters[i].Value?.ToString();

                        pr = $"{name}={value} {pr}";
                    }
                pr = $"(SQL Parameters: {pr.Trim()})";

                var arguments = new object[] { cp + ct + cd + rs + pr, executionTime, null, "BaseDataBase", commandText, 0 };
                typeof(Log).GetMethod(logLevel).Invoke(null, arguments);
            }

            return;
        }
        private static QueryResult Execute(string commandText, CommandType commandType, SqlParameter[] parameters)
        {
            QueryResult result;
            var dS = new DataSet();
            var sw = Stopwatch.StartNew();
            var returnCodeID = parameters.Length - 1;
            var resultTextID = parameters.Length - 2;
            var spResultCodeID = parameters.Length - 3;

            try
            {
                using (var connection = new SqlConnection(ProjectValues.DataBaseConfigure.ConnectionString))
                using (var command = new SqlCommand(commandText, connection) { CommandTimeout = ProjectValues.DataBaseConfigure.CommandTimeout, CommandType = commandType })
                using (var dataAdaptor = new SqlDataAdapter(command))
                {
                    command.Parameters.AddRange(parameters);

                    connection.Open();
                    dataAdaptor.Fill(dS);
                }

                if ((int)parameters[returnCodeID].Value != 1)
                {
                    result = new QueryResult { ReturnCode = (int)parameters[returnCodeID].Value, Text = (string)parameters[resultTextID].Value };
                    CreateLogMessage("Error", sw.Elapsed.TotalMilliseconds, commandText, commandType, result.Text, result.ReturnCode, parameters);
                    return result;
                }
                if ((int)parameters[spResultCodeID].Value != 1)
                {
                    result = new QueryResult { SpCode = (int)parameters[spResultCodeID].Value, ReturnCode = (int)parameters[returnCodeID].Value, Text = (string)parameters[resultTextID].Value };
                    CreateLogMessage("Error", sw.Elapsed.TotalMilliseconds, commandText, commandType, result.Text, result.SpCode, parameters);
                    return result;
                }
                result = new QueryResult { DataSet = dS, SpCode = (int)parameters[spResultCodeID].Value, ReturnCode = (int)parameters[returnCodeID].Value, Text = (string)parameters[resultTextID].Value };
                CreateLogMessage("Debug", sw.Elapsed.TotalMilliseconds, commandText, commandType, result.Text, 1, parameters);
                return result;
            }
            catch (Exception ex)
            {
                result = new QueryResult { Text = ex.Message.ToString(), ReturnCode = ex.HResult };
                CreateLogMessage("Fatal", sw.Elapsed.TotalMilliseconds, commandText, commandType, ex.ToString(), ex.HResult, parameters);
                return result;
            }
        }
        private static QueryResult ExecuteStoredProcedure(string storedProcedureName, List<SqlParameter> sqlParameters)
        {
            sqlParameters.Add(new SqlParameter { ParameterName = "@output_status", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output });
            sqlParameters.Add(new SqlParameter { ParameterName = "@output_message", SqlDbType = SqlDbType.NVarChar, Size = 4000, Direction = ParameterDirection.Output });
            sqlParameters.Add(new SqlParameter { ParameterName = "@returnvalue", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.ReturnValue });

            return Execute(storedProcedureName, CommandType.StoredProcedure, sqlParameters.ToArray());
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