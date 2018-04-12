using GhabzinoBot;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Configuration;
using Telegram.Bot;

namespace Ayantech.WebService
{
    public static class ProjectValues
    {
        //Develop and Test
        public static bool UseLiveDatabase { get; private set; } = true;
        //Base ProjectValues
        public static string CryptographyKey { private set; get; }
        public static DataBaseConfigure DataBaseConfigure { private set; get; }
        public static string GeneralMessageFailure { private set; get; }
        public static ConcurrentDictionary<string, string> MessageDictionary { private set; get; }
        public static ConcurrentDictionary<string, LinkedList<MonitorValue>> MonitoringDictionary { private set; get; }
        public static int MonitoringMaxSize { private set; get; }
        public static bool MonitoringMode { private set; get; }
        public static string[] MonitoringSafeClientsList { private set; get; }
        public static string[] StoredProceduresBlockList { private set; get; }
        public static string SuccessfulLog { private set; get; }
        //ProjectValues
        public static string ProjectName { private set; get; } = "GhabzinoBot";
        //Bot ProjectValues
        public static string PublicToken { private set; get; }
        public static TelegramBotClient Bot { private set; get; }
        public static string BotToken { private set; get; }
        public static string EndpointConfigurationName { private set; get; }
        public static string ApplicationType { private set; get; }
        public static string ApplicationVersion { private set; get; }
        public static TelegramApi TelegramApi { get; private set; }
        //String ProjectValues (Messages, Buttons, Inline Buttons, UserInputs,...)
        //  //Buttons
        public static string WaterBillInquiry { get; private set; }
        public static string GasBillInquiry { get; private set; }
        public static string ElectricityBillInquiry { get; private set; }
        public static string TrafficFinesInquery { get; private set; }
        public static string MciMobileBillInquiry { get; private set; }
        public static string FixedLineBillInquiry { get; set; }
        public static string All { get; private set; }

        public static string WaterBillHistory { get; private set; }
        public static string GasBillHistory { get; private set; }
        public static string ElectricityBillHistory { get; private set; }
        public static string TrafficFinesHistory { get; private set; }
        public static string MciMobileBillHistory { get; private set; }
        public static string FixedLineBillHistory { get; set; }

        public static string paymensts { get; private set; }
        public static string history { get; private set; }
        public static string bill { get; set; }
        public static string payBill { get; private set; }
        public static string payBills { get; private set; }
        public static string payBillHalfTerm { get; private set; }
        public static string payBillFullTerm { get; private set; }
        public static string payAllTraffic { get; private set; }
        public static string chooseToAdd { get; private set; }
        //  //Bill Inquiry - Step: None
        public static string WaterBillInquiryNone { get; private set; }
        public static string GasBillInquiryNone { get; private set; }
        public static string ElectricityBillInquiryNone { get; private set; }
        public static string MciMobileBillInquiryNone { get; private set; }
        public static string FixedLineBillInquiryNone { get; private set; }
        public static string TrafficFinesInquiryNone { get; private set; }
        //  //Bill Inquiry - Step: Inquired
        public static string payOnline { get; private set; }
        public static string ChooseToPayOnline { get; private set; }
        //  //Other
        public static string WebserviceStatusSuccess { get; private set; }
        public static string start { get; private set; }
        public static string returnToMainMenu { get; private set; }
        public static string MainMenuDescription { get; private set; }
        //  //Payment
        public static string PayInlineButton { get; private set; }
        public static string PayInlineButtons { get; private set; }
        public static string OrReturnToMainMenu { get; private set; }
        ////Bot Keyboards ProjectValues
        //private static KeyboardButton _btnReturnToMainMenu { get; set; }
        //private static KeyboardButton _btnGoToPaymentPage { get; set; }
        //private static KeyboardButton _btnGoToPaymentPageMidTerm { get; set; }
        //private static KeyboardButton _btnGoToPaymentPageFullTerm { get; set; }
        //private static KeyboardButton _btnGoToOnlinePayment { get; set; }

        //private static KeyboardButton _btnWaterBillInquiry { get; set; }
        //private static KeyboardButton _btnGasBillInquiry { get; set; }
        //private static KeyboardButton _btnElectricityBillInquiry { get; set; }

        //private static KeyboardButton _btnTrafficFinesInquiry { get; set; }
        //private static KeyboardButton _btnMciMobileBillInquiry { get; set; }
        //private static KeyboardButton _btnFixedLineBillInquiry { get; set; }

        //private static KeyboardButton _btnBills { get; set; }
        //private static KeyboardButton _btnHistory { get; set; }
        //private static KeyboardButton _btnRecipt { get; set; }
        ////Bot Inline Keyboards ProjectValues
        //private static InlineKeyboardCallbackButton _btnInlineReturnToMainMenu { get; set; }
        //private static InlineKeyboardUrlButton _btnInlineGoToPaymentPage { get; set; }
        //private static InlineKeyboardUrlButton _btnInlineGoToPaymentPageMidTerm { get; set; }
        //private static InlineKeyboardUrlButton _btnInlineGoToPaymentPageFullTerm { get; set; }

        //private static InlineKeyboardCallbackButton _btnInlineWaterBillInquiry { get; set; }
        //private static InlineKeyboardCallbackButton _btnInlineGasBillInquiry { get; set; }
        //private static InlineKeyboardCallbackButton _btnInlineElectricityBillInquiry { get; set; }

        //private static InlineKeyboardCallbackButton _btnInlineTrafficFinesInquiry { get; set; }
        //private static InlineKeyboardCallbackButton _btnInlineMciMobileBillInquiry { get; set; }
        //private static InlineKeyboardCallbackButton _btnInlineFixedLineBillInquiry { get; set; }

        //private static InlineKeyboardCallbackButton _btnInlineBills { get; set; }
        //private static InlineKeyboardCallbackButton _btnInlineHistory { get; set; }
        //private static InlineKeyboardCallbackButton _btnInlineRecipt { get; set; }



        static ProjectValues()
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var status = InitFixedValues();
                if (status == false)
                {
                    HttpRuntime.UnloadAppDomain();
                    return;
                }

                status = InitDataBase();
                if (status == false)
                {
                    HttpRuntime.UnloadAppDomain();
                    return;
                }

                status = InitOtherObjects();
                if (status == false)
                {
                    HttpRuntime.UnloadAppDomain();
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                HttpRuntime.UnloadAppDomain();
                return;
            }
        }
        private static bool InitDataBase()
        {
            var dataBaseConfigure = new DataBaseConfigure();
            var status = dataBaseConfigure.GetDataBaseConfigure();
            if (status == false)
                return false;

            DataBaseConfigure = dataBaseConfigure;
            return true;
        }
        private static bool InitFixedValues()
        {
            CryptographyKey = @"1234512345678976";
            MessageDictionary = new ConcurrentDictionary<string, string>();
            MonitoringDictionary = new ConcurrentDictionary<string, LinkedList<MonitorValue>>();

            SuccessfulLog = WebConfigurationManager.AppSettings["SuccessfulLog"];
            GeneralMessageFailure = WebConfigurationManager.AppSettings["GeneralMessageFailure"];
            MonitoringMode = Convert.ToBoolean(WebConfigurationManager.AppSettings["MonitoringMode"]);
            MonitoringMaxSize = Convert.ToInt32(WebConfigurationManager.AppSettings["MonitoringMaxSize"]);
            MonitoringSafeClientsList = WebConfigurationManager.AppSettings["MonitoringSafeClientsList"].Split(',');
            StoredProceduresBlockList = WebConfigurationManager.AppSettings["StoredProceduresBlockList"].Split(',');
            return true;
        }
        public static bool InitOtherObjects()
        {
            //Order of execution is necessary
            var status = InitStrings();
            if (status == false)
            {
                return false;
            }

            status = InitOtherFixedValues();
            if (status == false)
            {
                return false;
            }

            return true;
        }
        public static bool InitOtherFixedValues()
        {
            PublicToken = WebConfigurationManager.AppSettings["PublicToken"];
            BotToken = WebConfigurationManager.AppSettings["BotToken"];
            EndpointConfigurationName = WebConfigurationManager.AppSettings["EndpointConfigurationName"];
            Bot = new TelegramBotClient(BotToken);
            ApplicationType = "TelegramBot";
            ApplicationVersion = "1.0.0";
            TelegramApi = new TelegramApi();

            return true;
        }
        public static bool InitStrings()
        {
            //  //Buttons
            WaterBillInquiry = "آب";
            GasBillInquiry = "گاز";
            ElectricityBillInquiry = "برق";
            TrafficFinesInquery = "خلافی";
            MciMobileBillInquiry = "تلفن همراه";
            FixedLineBillInquiry = "تلفن ثابت";
            All = "همه سوابق";
            WaterBillHistory = "سوابق آب";
            GasBillHistory = "سوابق گاز";
            ElectricityBillHistory = "سوابق برق";
            TrafficFinesHistory = "سوابق خلافی";
            MciMobileBillHistory = "سوابق تلفن همراه";
            FixedLineBillHistory = "سوابق تلفن ثابت";
            paymensts = "پرداختی ها";
            history = "سوابق";
            bill = "قبض";
            payBill = "پرداخت قبض";
            payBills = "پرداخت قبوض";
            payBillHalfTerm = "پرداخت قبض میان دوره";
            payBillFullTerm = "پرداخت قبض پایان دوره";
            payAllTraffic = "پرداخت قبض همه جرایم";
            chooseToAdd = "انتخاب جرایم برای پرداخت";
            //  //Bill Inquiry - Step: None
            WaterBillInquiryNone = "لطفا شناسه قبض آب را وارد کنید یا به صفحه اصلی باز گردید:";
            GasBillInquiryNone = "لطفا شناسه قبض گاز را وارد کنید یا به صفحه اصلی باز گردید:";
            ElectricityBillInquiryNone = "لطفا شناسه قبض برق را وارد کنید یا به صفحه اصلی باز گردید:";
            MciMobileBillInquiryNone = "لطفا شماره تلفن همراه را وارد کنید یا به صفحه اصلی باز گردید:";
            FixedLineBillInquiryNone = "لطفا شماره تلفن ثابت را وارد کنید یا به صفحه اصلی باز گردید:";
            TrafficFinesInquiryNone = "لطفا شماره بارکد کارت ماشین را وارد کنید یا به صفحه اصلی باز گردید:";
            //  //Bill Inquiry - Step: Inquired
            payOnline = "اضافه کردن به لیست پرداخت آنلاین";
            ChooseToPayOnline = "انتخاب برای پرداخت آنلاین";
            //  //Payment
            PayInlineButton = "برای پرداخت از دکمه زیر استفاده کنید:";
            PayInlineButtons = "برای پرداخت از دکمه های زیر استفاده کنید:";
            OrReturnToMainMenu = "یا به صفحه اصلی باز گردید:";
            //  //Other
            WebserviceStatusSuccess = "G00000";
            start = "/start";
            returnToMainMenu = "بازگشت به صفحه اصلی";
            MainMenuDescription = "لطفا برای ادامه یکی از موارد را انتخاب کنید:";
            return true;
        }

        //public static bool InitBotButtons()
        //{
        //            //Bot Keyboards ProjectValues
        //private static KeyboardButton _btnReturnToMainMenu { get; set; }
        //private static KeyboardButton _btnGoToPaymentPage { get; set; }
        //private static KeyboardButton _btnGoToPaymentPageMidTerm { get; set; }
        //private static KeyboardButton _btnGoToPaymentPageFullTerm { get; set; }
        //private static KeyboardButton _btnGoToOnlinePayment { get; set; }

        //private static KeyboardButton _btnWaterBillInquiry { get; set; }
        //private static KeyboardButton _btnGasBillInquiry { get; set; }
        //private static KeyboardButton _btnElectricityBillInquiry { get; set; }

        //private static KeyboardButton _btnTrafficFinesInquiry { get; set; }
        //private static KeyboardButton _btnMciMobileBillInquiry { get; set; }
        //private static KeyboardButton _btnFixedLineBillInquiry { get; set; }

        //private static KeyboardButton _btnBills { get; set; }
        //private static KeyboardButton _btnHistory { get; set; }
        //private static KeyboardButton _btnRecipt { get; set; }
        ////Bot Inline Keyboards ProjectValues
        //private static InlineKeyboardCallbackButton _btnInlineReturnToMainMenu { get; set; }
        //private static InlineKeyboardUrlButton _btnInlineGoToPaymentPage { get; set; }
        //private static InlineKeyboardUrlButton _btnInlineGoToPaymentPageMidTerm { get; set; }
        //private static InlineKeyboardUrlButton _btnInlineGoToPaymentPageFullTerm { get; set; }

        //private static InlineKeyboardCallbackButton _btnInlineWaterBillInquiry { get; set; }
        //private static InlineKeyboardCallbackButton _btnInlineGasBillInquiry { get; set; }
        //private static InlineKeyboardCallbackButton _btnInlineElectricityBillInquiry { get; set; }

        //private static InlineKeyboardCallbackButton _btnInlineTrafficFinesInquiry { get; set; }
        //private static InlineKeyboardCallbackButton _btnInlineMciMobileBillInquiry { get; set; }
        //private static InlineKeyboardCallbackButton _btnInlineFixedLineBillInquiry { get; set; }

        //private static InlineKeyboardCallbackButton _btnInlineBills { get; set; }
        //private static InlineKeyboardCallbackButton _btnInlineHistory { get; set; }
        //private static InlineKeyboardCallbackButton _btnInlineRecipt { get; set; }

        //    return true;
        //}
    }
}