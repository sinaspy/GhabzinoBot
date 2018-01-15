using Ayantech.WebService;
using GhabzinoBot.GhabzinoService;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace GhabzinoBot
{
    public static class DataHandler
    {
        public class PaymentStructure
        {
            public long userId { get; set; }
            //public string StrToken { get; set; }
            public ReportNewPaymentInputParams[] RnpipArray { get; set; }
        }
        public class SelectedBillsStructure
        {
            public string StrToken { get; set; }
            public int[] SelectedBills { get; set; }
        }
        public class TerraficFinesStructure
        {
            public string StrToken { get; set; }
            public TerraficFines[] TerraficFinesArray { get; set; }
        }
        public class TerraficFines
        {
            public string City { get; set; }
            public string DateTime { get; set; }
            public string Delivery { get; set; }
            public string Location { get; set; }
            public string Type { get; set; }
            public long Amount { get; set; }
        }
        public class TerraficFinesPageStructure
        {
            public string StrToken { get; set; }
            public int TerraficFinesPage { get; set; }
        }

        public static UserInfo ReadUserInfo(long userId)
        {
            var sw = Stopwatch.StartNew();

            if (ProjectValues.UseLiveDatabase)
            {
                var result = DataBase.GetTelegramUserInfo(userId);
                if (result.HasError)
                {
                    Ayantech.WebService.Log.Error("DB: SP Has Error", sw.Elapsed.TotalMilliseconds);
                    return null;
                }
                else if (result?.DataSet?.Tables[0].Rows.Count == 0)
                {
                    Ayantech.WebService.Log.Info("DB: No result returned", sw.Elapsed.TotalMilliseconds);
                    return new UserInfo { UserId = userId };
                }

                var dbArray = result.DataSet.NullCheck().Tables[0].Rows[0].ItemArray;
                Ayantech.WebService.Log.Trace(JsonConvert.SerializeObject(dbArray), sw.Elapsed.TotalMilliseconds);
                //var dbInfo = new UserInfo { UserId = (long)dbArray[0], Mobile = dbArray[1].ToString(), Token = dbArray[2].ToString(), UserState = (UserState)dbArray[3], UserField = (UserField)dbArray[4], WaterBillInquiryStep = (WaterBillInquiryStep)dbArray[5], GasBillInquiryStep = (GasBillInquiryStep)dbArray[6], ElectricityBillInquiryStep = (ElectricityBillInquiryStep)dbArray[7], MciMobileBillInquiryStep = (MciMobileBillInquiryStep)dbArray[8], FixedLineBillInquiryStep = (FixedLineBillInquiryStep)dbArray[9], TrafficFinesInquiryStep = (TrafficFinesInquiryStep)dbArray[10], BillStep = (BillStep)dbArray[11], HistoryStep = (HistoryStep)dbArray[12] };
                var dbInfo = new UserInfo
                {
                    UserId = (long)dbArray[0],
                    Mobile = dbArray[1].ToString(),
                    Token = dbArray[2].ToString(),
                    UserState = (UserState)dbArray[3],
                    UserField = (UserField)dbArray[4],
                    WaterBillInquiryStep = (WaterBillInquiryStep)dbArray[5],
                    GasBillInquiryStep = (GasBillInquiryStep)dbArray[6],
                    ElectricityBillInquiryStep = (ElectricityBillInquiryStep)dbArray[7],
                    MciMobileBillInquiryStep = (MciMobileBillInquiryStep)dbArray[8],
                    FixedLineBillInquiryStep = (FixedLineBillInquiryStep)dbArray[9],
                    TrafficFinesInquiryStep = (TrafficFinesInquiryStep)dbArray[10],
                    BillStep = (BillStep)dbArray[11],
                    HistoryStep = (HistoryStep)dbArray[12]
                };

                return dbInfo;
            }
            else
            {
                using (File.AppendText(@"C:\Users\Resurrection\Desktop\userinfo.txt")) ;

                var fileContent = File.ReadAllLines(@"C:\Users\Resurrection\Desktop\userinfo.txt");

                UserInfo LastUserRecord = new UserInfo() { UserId = userId };

                foreach (var record in fileContent)
                {
                    var userRecord = JsonConvert.DeserializeObject<UserInfo>(record);
                    if (userId == userRecord.UserId)
                    {
                        LastUserRecord = userRecord;
                    }
                }

                return LastUserRecord;
            }
        }
        public static bool SaveUserInfo(UserInfo userInfo)
        {
            var sw = Stopwatch.StartNew();

            if (ProjectValues.UseLiveDatabase)
            {
                var result = DataBase.SetTelegramUserInfo(userInfo.UserId, userInfo.Mobile, userInfo.Token, (int)userInfo.UserState, (int)userInfo.UserField, (int)userInfo.WaterBillInquiryStep, (int)userInfo.GasBillInquiryStep, (int)userInfo.ElectricityBillInquiryStep, (int)userInfo.MciMobileBillInquiryStep, (int)userInfo.FixedLineBillInquiryStep, (int)userInfo.TrafficFinesInquiryStep, (int)userInfo.BillStep, (int)userInfo.HistoryStep);
                if (result.HasError)
                {
                    Ayantech.WebService.Log.Error("DB: SP Has Error", sw.Elapsed.TotalMilliseconds);
                    return false;
                }
                //else if (result?.DataSet?.Tables[0].Rows.Count == 0)
                //{
                //    Ayantech.WebService.Log.Info("DB: No result returned", sw.Elapsed.TotalMilliseconds);
                //    return false;
                //}

                return true;
            }
            else
            {
                var user = JsonConvert.SerializeObject(userInfo);
                File.AppendAllLines(@"C:\Users\Resurrection\Desktop\userinfo.txt", new string[] { user });

                return true;
            }
        }
        public static ReportNewPaymentInputParams[] ReadPaymentInfo(long userId)
        {
            var sw = Stopwatch.StartNew();

            if (ProjectValues.UseLiveDatabase)
            {
                var result = DataBase.GetTelegramPaymentInfo(userId);
                if (result.HasError)
                {
                    Ayantech.WebService.Log.Error("DB: SP Has Error", sw.Elapsed.TotalMilliseconds);
                    return null;
                }
                else if (result?.DataSet?.Tables[0].Rows.Count == 0)
                {
                    Ayantech.WebService.Log.Info("DB: No result returned", sw.Elapsed.TotalMilliseconds);
                    return null;
                }

                ReportNewPaymentInputParams[] PaymentRecords = null;
                if (result.DataSet.Tables[0].Rows.Count > 0)
                {
                    PaymentRecords = new ReportNewPaymentInputParams[result.DataSet.Tables[0].Rows.Count];
                    Ayantech.WebService.Log.Trace("Number of payment records: " + result.DataSet.Tables[0].Rows.Count, sw.Elapsed.TotalMilliseconds);
                }

                for (int i = 0; i < result.DataSet.Tables[0].Rows.Count; i++)
                {
                    var dbArray = result.DataSet.Tables[0].Rows[i].ItemArray;
                    Ayantech.WebService.Log.Trace(JsonConvert.SerializeObject(dbArray), sw.Elapsed.TotalMilliseconds);
                    var paymentRecord = new ReportNewPaymentInputParams { BillID = dbArray[0].ToString(), PaymentID = dbArray[1].ToString() };
                    PaymentRecords[i] = paymentRecord;

                }

                return PaymentRecords;
            }
            else
            {
                using (File.AppendText(@"C:\Users\Resurrection\Desktop\payment.txt")) ;

                var fileContent = File.ReadAllLines(@"C:\Users\Resurrection\Desktop\payment.txt");

                ReportNewPaymentInputParams[] LastPaymentRecord = null;

                foreach (var record in fileContent)
                {
                    var currentRecord = JsonConvert.DeserializeObject<PaymentStructure>(record);
                    if (currentRecord.userId == userId)
                    {
                        LastPaymentRecord = currentRecord.RnpipArray;

                    }
                }

                return LastPaymentRecord;
            }
        }
        public static bool SavePaymentInfo(long userId, params ReportNewPaymentInputParams[] payments)
        {
            var sw = Stopwatch.StartNew();

            if (ProjectValues.UseLiveDatabase)
            {
                DataTable dt = new DataTable("TelegramPaymentInfoTableType");
                dt.Columns.Add("billID", typeof(string));
                dt.Columns.Add("paymentID", typeof(string));

                foreach (var payment in payments)
                {
                    dt.Rows.Add(new object[] { payment.BillID, payment.PaymentID });
                }


                var result = DataBase.SetTelegramPaymentInfo(userId, dt);
                if (result.HasError)
                {
                    Ayantech.WebService.Log.Error("DB: SP Has Error", sw.Elapsed.TotalMilliseconds);
                    return false;
                }
                //else if (result?.DataSet?.Tables[0].Rows.Count == 0)
                //{
                //    Ayantech.WebService.Log.Info("DB: No result returned", sw.Elapsed.TotalMilliseconds);
                //    return false;
                //}



                return true;
            }
            else
            {




                var str = JsonConvert.SerializeObject(new PaymentStructure { userId = userId, RnpipArray = payments });
                File.AppendAllLines(@"C:\Users\Resurrection\Desktop\payment.txt", new string[] { str });


            }
            return true;
        }


        public static TerraficFines[] ReadTerraficFinesInfo(string token, int? recordNumber = null)
        {
            using (File.AppendText(@"C:\Users\Resurrection\Desktop\terrafic.txt")) ;

            var fileContent = File.ReadAllLines(@"C:\Users\Resurrection\Desktop\terrafic.txt");

            TerraficFines[] LastTerraficFinesRecord = null;

            foreach (var record in fileContent)
            {
                var currentRecord = JsonConvert.DeserializeObject<TerraficFinesStructure>(record);
                if (currentRecord.StrToken == token)
                {
                    LastTerraficFinesRecord = currentRecord.TerraficFinesArray;
                }
            }

            if (recordNumber != null)
            {
                return new TerraficFines[] { LastTerraficFinesRecord[Convert.ToInt32(recordNumber)] };
            }
            else
            {
                var selectedBills = ReadSelectedBillsInfo(token);
                var temp = new TerraficFines[selectedBills.Length];
                for (int i = 0; i < selectedBills.Length; i++)
                {
                    temp[i] = LastTerraficFinesRecord[selectedBills[i]];
                }
                LastTerraficFinesRecord = temp;
            }

            return LastTerraficFinesRecord;
        }
        public static bool SaveTerraficFinesInfo(string token, params TerraficFines[] terraficFinesDetails)
        {
            var str = JsonConvert.SerializeObject(new TerraficFinesStructure { StrToken = token, TerraficFinesArray = terraficFinesDetails });
            File.AppendAllLines(@"C:\Users\Resurrection\Desktop\terrafic.txt", new string[] { str });

            return true;
        }


        public static int[] ReadSelectedBillsInfo(string token)
        {
            using (File.AppendText(@"C:\Users\Resurrection\Desktop\terrafic.txt")) ;

            var fileContent = File.ReadAllLines(@"C:\Users\Resurrection\Desktop\terrafic.txt");

            int[] LastSelectedBillsRecord = null;

            foreach (var record in fileContent)
            {
                var currentRecord = JsonConvert.DeserializeObject<SelectedBillsStructure>(record);
                if (currentRecord.StrToken == token)
                {
                    LastSelectedBillsRecord = currentRecord.SelectedBills;
                }
            }

            return LastSelectedBillsRecord;
        }
        public static bool SaveSelectedBillsInfo(string token, params int[] selectedBills)
        {
            var str = JsonConvert.SerializeObject(new SelectedBillsStructure { StrToken = token, SelectedBills = selectedBills });
            File.AppendAllLines(@"C:\Users\Resurrection\Desktop\terrafic.txt", new string[] { str });

            return true;
        }

        public static int ReadTerraficFinesPage(string token)
        {
            using (File.AppendText(@"C:\Users\Resurrection\Desktop\TerraficFinePage.txt")) ;

            var fileContent = File.ReadAllLines(@"C:\Users\Resurrection\Desktop\TerraficFinePage.txt");

            int LastPageRecord = 0;

            foreach (var record in fileContent)
            {
                var currentRecord = JsonConvert.DeserializeObject<TerraficFinesPageStructure>(record);
                if (currentRecord.StrToken == token)
                {
                    LastPageRecord = currentRecord.TerraficFinesPage;
                }
            }

            return LastPageRecord;
        }
        public static bool SaveTerraficFinesPage(string token, int page)
        {
            var str = JsonConvert.SerializeObject(new TerraficFinesPageStructure { StrToken = token, TerraficFinesPage = page });
            File.AppendAllLines(@"C:\Users\Resurrection\Desktop\TerraficFinePage.txt", new string[] { str });

            return true;
        }


        public static bool Log(object record)
        {
            var str = JsonConvert.SerializeObject(record);
            File.AppendAllLines(@"C:\Users\Resurrection\Desktop\log.txt", new string[] { str });
            return true;
        }
    }
}
