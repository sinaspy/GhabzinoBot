using GhabzinoBot.GhabzinoService;
using Newtonsoft.Json;
using System;
using System.IO;

namespace GhabzinoBot
{
    public static class DataHandler
    {
        public class PaymentStructure
        {
            public string StrToken { get; set; }
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

        public static UserInfo ReadUserInfo(int userID)
        {
            using (File.AppendText(@"C:\Users\Resurrection\Desktop\userinfo.txt")) ;

            var fileContent = File.ReadAllLines(@"C:\Users\Resurrection\Desktop\userinfo.txt");

            UserInfo LastUserRecord = new UserInfo() { UserID = userID };

            foreach (var record in fileContent)
            {
                var userRecord = JsonConvert.DeserializeObject<UserInfo>(record);
                if (userID == userRecord.UserID)
                {
                    LastUserRecord = userRecord;
                }
            }

            return LastUserRecord;
        }
        public static bool SaveUserInfo(UserInfo userInfo)
        {
            var user = JsonConvert.SerializeObject(userInfo);
            File.AppendAllLines(@"C:\Users\Resurrection\Desktop\userinfo.txt", new string[] { user });

            return true;
        }
        public static ReportNewPaymentInputParams[] ReadPaymentInfo(string token)
        {
            using (File.AppendText(@"C:\Users\Resurrection\Desktop\payment.txt")) ;

            var fileContent = File.ReadAllLines(@"C:\Users\Resurrection\Desktop\payment.txt");

            ReportNewPaymentInputParams[] LastPaymentRecord = null;

            foreach (var record in fileContent)
            {
                var currentRecord = JsonConvert.DeserializeObject<PaymentStructure>(record);
                if (currentRecord.StrToken == token)
                {
                    LastPaymentRecord = currentRecord.RnpipArray;

                }
            }

            return LastPaymentRecord;
        }
        public static bool SavePaymentInfo(string token, params ReportNewPaymentInputParams[] payments)
        {
            var str = JsonConvert.SerializeObject(new PaymentStructure { StrToken = token, RnpipArray = payments });
            File.AppendAllLines(@"C:\Users\Resurrection\Desktop\payment.txt", new string[] { str });

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
