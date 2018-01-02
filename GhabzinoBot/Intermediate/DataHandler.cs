using GhabzinoBot.GhabzinoService;
using Newtonsoft.Json;
using System.IO;

namespace GhabzinoBot
{
    public static class DataHandler
    {
        public class PaymentStructure
        {
            public string StrToken { get; set; }
            //public bool[] ValidForPayment { get; set; }
            public ReportNewPaymentInputParams[] Rnpip { get; set; }
        }

        public static UserInfo ReadUserInfo(int userID)
        {
            using (File.AppendText(@"C:\Users\Resurrection\Desktop\userinfo.txt")) ;

            var fileContent = File.ReadAllLines(@"C:\Users\Resurrection\Desktop\userinfo.txt");

            UserInfo LastUserRecord = new UserInfo() { /*UserState = UserState.Registered,*/ UserID = userID };//TEMP 

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
        public static ReportNewPaymentInputParams[] ReadPaymentInfo(string token/*, out bool[] validForPayment*/)
        {
            using (File.AppendText(@"C:\Users\Resurrection\Desktop\payment.txt")) ;

            var fileContent = File.ReadAllLines(@"C:\Users\Resurrection\Desktop\payment.txt");

            ReportNewPaymentInputParams[] LastPaymentRecord = null;
            //validForPayment = null;

            foreach (var record in fileContent)
            {
                var currentRecord = JsonConvert.DeserializeObject<PaymentStructure>(record);
                if (currentRecord.StrToken == token)
                {
                    LastPaymentRecord = currentRecord.Rnpip;
                    //validForPayment = new bool[currentRecord.ValidForPayment.Length];
                    //for (int i = 0; i < currentRecord.ValidForPayment.Length; i++)
                    //{
                    //    validForPayment[i] = currentRecord.ValidForPayment[i];
                    //}
                }
            }

            return LastPaymentRecord;
        }
        public static bool SavePaymentInfo(string token/*, bool[] validForPayment*/, params ReportNewPaymentInputParams[] payments)
        {
            var str = JsonConvert.SerializeObject(new PaymentStructure { StrToken = token/*, ValidForPayment = validForPayment*/, Rnpip = payments });
            File.AppendAllLines(@"C:\Users\Resurrection\Desktop\payment.txt", new string[] { str });

            return true;
        }
        //public static ReportNewPaymentInputParams[] ReadAndRemovePaymentInfo(string token)
        //{
        //    using (File.AppendText(@"C:\Users\Resurrection\Desktop\payment.txt")) ;

        //    var fileContent = File.ReadAllLines(@"C:\Users\Resurrection\Desktop\payment.txt");

        //    ReportNewPaymentInputParams[] LastPaymentRecord = null;

        //    for (int i = 0; i < fileContent.Length; i++)
        //    {
        //        var record = fileContent[i];
        //        var currentRecord = JsonConvert.DeserializeObject<PaymentStructure>(record);
        //        if (currentRecord.StrToken == token && currentRecord.ValidForPayment)
        //        {
        //            LastPaymentRecord = currentRecord.Rnpip;
        //            File.Delete(@"C:\Users\Resurrection\Desktop\payment.txt");
        //            for (int j = 0; j < fileContent.Length - 1; j++)
        //            {
        //                if (j == i)
        //                {
        //                    i++;
        //                }
        //                File.AppendAllLines(@"C:\Users\Resurrection\Desktop\payment.txt", new string[] { fileContent[i] });
        //            }
        //            break;
        //        }
        //    }

        //    return LastPaymentRecord;
        //}
        public static bool Log(object record)
        {
            var str = JsonConvert.SerializeObject(record);
            File.AppendAllLines(@"C:\Users\Resurrection\Desktop\log.txt", new string[] { str });
            return true;
        }
    }
}
