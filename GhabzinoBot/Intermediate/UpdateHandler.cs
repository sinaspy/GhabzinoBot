using Ayantech.WebService;
using GhabzinoBot.GhabzinoService;
using System;
using System.Diagnostics;
using System.Linq;
using Telegram.Bot.Types;

namespace GhabzinoBot
{
    public class UpdateHandler
    {
        public UserInfo UserInfo { get; set; }
        public Update Update { get; set; }

        public void Bot_OnUpdate()
        {
            var sw = Stopwatch.StartNew();

            if (!(Update.Type == Telegram.Bot.Types.Enums.UpdateType.MessageUpdate || Update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQueryUpdate))
            {
                Log.Warn($"(Update ID: {Update.Id})(Wrong Update Type: {Update.Type.ToString()})", sw.Elapsed.TotalMilliseconds);//???
                return;
            }

            int userId = Update?.Message?.From?.Id == null ? 0 : Update.Message.From.Id;
            userId = Update?.CallbackQuery?.From?.Id == null ? userId : Update.CallbackQuery.From.Id;
            UserInfo = DataHandler.ReadUserInfo(userId);

            var messageText = Update?.Message?.Text != null ? Update.Message.Text : Update.CallbackQuery.Data;

            if (UserInfo.UserState != UserState.Registered)
            {
                Register(messageText);
                return;
            }

            switch (messageText.ToLower())
            {
                case ConstantStrings.start:
                case ConstantStrings.returnToMainMenu:
                    UserInfo.UserField = UserField.None;
                    TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.MainMenu, ConstantStrings.MainMenuDescription);
                    break;

                case ConstantStrings.WaterBillInquiry:
                    UserInfo.UserField = UserField.WaterBillInquiry;
                    UserInfo.WaterBillInquiryStep = WaterBillInquiryStep.None;
                    WaterBillInquiry();
                    break;

                case ConstantStrings.GasBillInquiry:
                    UserInfo.UserField = UserField.GasBillInquiry;
                    UserInfo.GasBillInquiryStep = GasBillInquiryStep.None;
                    GasBillInquiry();
                    break;

                case ConstantStrings.ElectricityBillInquiry:
                    UserInfo.UserField = UserField.ElectricityBillInquiry;
                    UserInfo.ElectricityBillInquiryStep = ElectricityBillInquiryStep.None;
                    ElectricityBillInquiry();
                    break;

                case ConstantStrings.MciMobileBillInquiry:
                    UserInfo.UserField = UserField.MciMobileBillInquiry;
                    UserInfo.MciMobileBillInquiryStep = MciMobileBillInquiryStep.None;
                    MciMobileBillInquiry();
                    break;

                case ConstantStrings.FixedLineBillInquiry:
                    UserInfo.UserField = UserField.FixedLineBillInquiry;
                    UserInfo.FixedLineBillInquiryStep = FixedLineBillInquiryStep.None;
                    FixedLineBillInquiry();
                    break;

                case ConstantStrings.TrafficFinesInquery:
                    UserInfo.UserField = UserField.TrafficFinesInquery;
                    UserInfo.TrafficFinesInquiryStep = TrafficFinesInquiryStep.None;
                    TrafficFinesInquiry();
                    break;

                case ConstantStrings.paymensts:

                    break;
                case ConstantStrings.history:
                    //
                    break;
                case ConstantStrings.bill:
                    UserInfo.UserField = UserField.Bill;
                    UserInfo.BillStep = BillStep.None;
                    Bill();
                    break;

                default:
                    ProcessRequest(messageText);
                    break;
            }

            DataHandler.SaveUserInfo(UserInfo);
        }
        private void ProcessRequest(string messageText)
        {
            switch (UserInfo.UserField)
            {
                case UserField.None:
                    //???
                    break;
                case UserField.WaterBillInquiry:
                    WaterBillInquiry(messageText);
                    break;
                case UserField.GasBillInquiry:
                    GasBillInquiry(messageText);
                    break;
                case UserField.ElectricityBillInquiry:
                    ElectricityBillInquiry(messageText);
                    break;
                case UserField.MciMobileBillInquiry:
                    MciMobileBillInquiry(messageText);
                    break;
                case UserField.FixedLineBillInquiry:
                    FixedLineBillInquiry(messageText);
                    break;
                case UserField.TrafficFinesInquery:
                    TrafficFinesInquiry(messageText);
                    break;
                case UserField.Bill:
                    Bill(messageText);
                    break;
                case UserField.History:
                    History(messageText);
                    break;
                case UserField.Payments:
                    break;
                default:
                    //???
                    break;
            }
        }
        private string FormatBillDetails(object inputObj)
        {
            string outputString = string.Empty;

            switch (UserInfo.UserField)
            {
                case UserField.None:
                    //???
                    break;

                case UserField.WaterBillInquiry:
                    WaterBillInquiryOutput waterbillObj = (WaterBillInquiryOutput)inputObj;
                    outputString += $"مبلغ: {Helper.ToPersianNumber(Helper.ToTomanCurrency(waterbillObj.Parameters.Amount))} تومان{System.Environment.NewLine}";
                    outputString += $"نام: {waterbillObj.Parameters.FullName}{System.Environment.NewLine}";
                    outputString += $"شناسه قبض: {Helper.ToPersianNumber(waterbillObj.Parameters.BillID)}{System.Environment.NewLine}";
                    outputString += $"شناسه پرداخت: {Helper.ToPersianNumber(waterbillObj.Parameters.PaymentID)}{System.Environment.NewLine}";
                    outputString += $"مهلت پرداخت: {Helper.ToPersianNumber(waterbillObj.Parameters.PaymentDate)}{System.Environment.NewLine}";
                    outputString += $"قرائت فعلی: {Helper.ToPersianNumber(waterbillObj.Parameters.PreviousDate)}{System.Environment.NewLine}";
                    outputString += $"قرائت قبلی: {Helper.ToPersianNumber(waterbillObj.Parameters.CurrentDate)}{System.Environment.NewLine}";
                    outputString += $"آدرس: {Helper.ToPersianNumber(waterbillObj.Parameters.Address)}{System.Environment.NewLine}";
                    outputString += System.Environment.NewLine;
                    //if (!waterbillObj.Parameters.ValidForPayment)
                    //{
                    //    outputString += "قبلا پرداخت شده است.";
                    //}
                    break;

                case UserField.GasBillInquiry:
                    GasBillInquiryOutput gasBillObj = (GasBillInquiryOutput)inputObj;
                    outputString += $"مبلغ: {Helper.ToPersianNumber(Helper.ToTomanCurrency(gasBillObj.Parameters.Amount))} تومان{System.Environment.NewLine}";
                    outputString += $"نام: {gasBillObj.Parameters.FullName}{System.Environment.NewLine}";
                    outputString += $"شناسه قبض: {Helper.ToPersianNumber(gasBillObj.Parameters.BillID)}{System.Environment.NewLine}";
                    outputString += $"شناسه پرداخت: {Helper.ToPersianNumber(gasBillObj.Parameters.PaymentID)}{System.Environment.NewLine}";
                    outputString += $"مهلت پرداخت: {Helper.ToPersianNumber(gasBillObj.Parameters.PaymentDate)}{System.Environment.NewLine}";
                    outputString += $"قرائت فعلی: {Helper.ToPersianNumber(gasBillObj.Parameters.PreviousDate)}{System.Environment.NewLine}";
                    outputString += $"قرائت قبلی: {Helper.ToPersianNumber(gasBillObj.Parameters.CurrentDate)}{System.Environment.NewLine}";
                    outputString += $"آدرس: {Helper.ToPersianNumber(gasBillObj.Parameters.Address)}{System.Environment.NewLine}";
                    outputString += System.Environment.NewLine;
                    //if (!gasBillObj.Parameters.ValidForPayment)
                    //{
                    //    outputString += "قبلا پرداخت شده است.";
                    //}
                    break;

                case UserField.ElectricityBillInquiry:
                    ElectricityBillInquiryOutput electricityBillObj = (ElectricityBillInquiryOutput)inputObj;
                    outputString += $"مبلغ: {Helper.ToPersianNumber(Helper.ToTomanCurrency(electricityBillObj.Parameters.Amount))} تومان{System.Environment.NewLine}";
                    outputString += $"نام: {electricityBillObj.Parameters.FullName}{System.Environment.NewLine}";
                    outputString += $"شناسه قبض: {Helper.ToPersianNumber(electricityBillObj.Parameters.BillID)}{System.Environment.NewLine}";
                    outputString += $"شناسه پرداخت: {Helper.ToPersianNumber(electricityBillObj.Parameters.PaymentID)}{System.Environment.NewLine}";
                    outputString += $"مهلت پرداخت: {Helper.ToPersianNumber(electricityBillObj.Parameters.PaymentDate)}{System.Environment.NewLine}";
                    outputString += $"قرائت فعلی: {Helper.ToPersianNumber(electricityBillObj.Parameters.PreviousDate)}{System.Environment.NewLine}";
                    outputString += $"قرائت قبلی: {Helper.ToPersianNumber(electricityBillObj.Parameters.CurrentDate)}{System.Environment.NewLine}";
                    outputString += $"آدرس: {Helper.ToPersianNumber(electricityBillObj.Parameters.Address)}{System.Environment.NewLine}";
                    outputString += System.Environment.NewLine;
                    //if (!electricityBillObj.Parameters.ValidForPayment)
                    //{
                    //    outputString += "قبلا پرداخت شده است.";
                    //}
                    break;

                case UserField.MciMobileBillInquiry:
                    MCIMobileBillInquiryOutput mciMobileBillObj = (MCIMobileBillInquiryOutput)inputObj;
                    outputString += $"پایان دوره{System.Environment.NewLine}";
                    //if (mciMobileBillObj.Parameters.FinalTerm.ValidForPayment)
                    //{
                    outputString += $"{Helper.ToPersianNumber(Helper.ToTomanCurrency(mciMobileBillObj.Parameters.FinalTerm.Amount))} تومان{System.Environment.NewLine}";
                    outputString += $"شناسه قبض: {Helper.ToPersianNumber(mciMobileBillObj.Parameters.FinalTerm.BillID)}{System.Environment.NewLine}";
                    outputString += $"شناسه پرداخت: {Helper.ToPersianNumber(mciMobileBillObj.Parameters.FinalTerm.PaymentID)}{System.Environment.NewLine}";
                    //}
                    //else
                    //{
                    //    outputString += "قبلا پرداخت شده است.";
                    //}
                    outputString += System.Environment.NewLine;
                    outputString += $"میان دوره{System.Environment.NewLine}";
                    //if (mciMobileBillObj.Parameters.MidTerm.ValidForPayment)
                    //{
                    outputString += $"{Helper.ToPersianNumber(Helper.ToTomanCurrency(mciMobileBillObj.Parameters.MidTerm.Amount))} تومان{System.Environment.NewLine}";
                    outputString += $"شناسه قبض: {Helper.ToPersianNumber(mciMobileBillObj.Parameters.MidTerm.BillID)}{System.Environment.NewLine}";
                    outputString += $"شناسه پرداخت: {Helper.ToPersianNumber(mciMobileBillObj.Parameters.MidTerm.PaymentID)}{System.Environment.NewLine}";
                    //}
                    //else
                    //{
                    //    outputString += "قبلا پرداخت شده است.";
                    //}
                    outputString += System.Environment.NewLine;
                    break;

                case UserField.FixedLineBillInquiry:
                    FixedLineBillInquiryOutput fixedLineBillObj = (FixedLineBillInquiryOutput)inputObj;
                    outputString += $"پایان دوره{System.Environment.NewLine}";
                    //if (fixedLineBillObj.Parameters.FinalTerm.ValidForPayment)
                    //{
                    outputString += $"{Helper.ToPersianNumber(Helper.ToTomanCurrency(fixedLineBillObj.Parameters.FinalTerm.Amount))} تومان{System.Environment.NewLine}";
                    outputString += $"شناسه قبض: {Helper.ToPersianNumber(fixedLineBillObj.Parameters.FinalTerm.BillID)}{System.Environment.NewLine}";
                    outputString += $"شناسه پرداخت: {Helper.ToPersianNumber(fixedLineBillObj.Parameters.FinalTerm.PaymentID)}{System.Environment.NewLine}";
                    //}
                    //else
                    //{
                    //    outputString += "قبلا پرداخت شده است.";
                    //}
                    outputString += System.Environment.NewLine;
                    outputString += $"میان دوره{System.Environment.NewLine}";
                    //if (fixedLineBillObj.Parameters.MidTerm.ValidForPayment)
                    //{
                    outputString += $"{Helper.ToPersianNumber(Helper.ToTomanCurrency(fixedLineBillObj.Parameters.MidTerm.Amount))} تومان{System.Environment.NewLine}";
                    outputString += $"شناسه قبض: {Helper.ToPersianNumber(fixedLineBillObj.Parameters.MidTerm.BillID)}{System.Environment.NewLine}";
                    outputString += $"شناسه پرداخت: {Helper.ToPersianNumber(fixedLineBillObj.Parameters.MidTerm.PaymentID)}{System.Environment.NewLine}";
                    //}
                    //else
                    //{
                    //    outputString += "قبلا پرداخت شده است.";
                    //}
                    outputString += System.Environment.NewLine;
                    break;

                case UserField.TrafficFinesInquery:
                    TrafficFinesInquiryOutput TrafficFinesObj = (TrafficFinesInquiryOutput)inputObj;
                    outputString += $"پلاک: {Helper.ToPersianNumber(TrafficFinesObj.Parameters.PlateNumber)}{System.Environment.NewLine}";
                    outputString += $"تعداد کل جریمه های قابل پرداخت: {Helper.ToPersianNumber(TrafficFinesObj.Parameters.TotalValidForPaymentCount.ToString())}{System.Environment.NewLine}";
                    outputString += $"مبلغ کل: {Helper.ToPersianNumber(Helper.ToTomanCurrency(TrafficFinesObj.Parameters.TotalAmount))} تومان{System.Environment.NewLine}";
                    outputString += System.Environment.NewLine;
                    //if (!electricityBillObj.Parameters.ValidForPayment)
                    //{
                    //    outputString += "قبلا پرداخت شده است.";
                    //}
                    break;

                case UserField.Bill:
                    ReportNewPaymentOutput BillObj = (ReportNewPaymentOutput)inputObj;
                    outputString += $"مبلغ: {Helper.ToPersianNumber(Helper.ToTomanCurrency(BillObj.Parameters.Bills[0].Amount))} تومان{Environment.NewLine}";
                    outputString += $"نوع قبض: {BillObj.Parameters.Bills[0].BillTypeShowName}{Environment.NewLine}";
                    outputString += $"شناسه قبض: {Helper.ToPersianNumber(BillObj.Parameters.Bills[0].BillID)}{Environment.NewLine}";
                    outputString += $"شناسه پرداخت: {Helper.ToPersianNumber(BillObj.Parameters.Bills[0].PaymentID)}{Environment.NewLine}";
                    outputString += Environment.NewLine;
                    //if (!electricityBillObj.Parameters.ValidForPayment)
                    //{
                    //    outputString += "قبلا پرداخت شده است.";
                    //}
                    break;

                case UserField.History:
                    break;
                case UserField.Payments:
                    break;
                default:
                    break;
            }

            return outputString;
        }
        private string FormatTerraficFinesDetails(int currentPage, int totalPage, DataHandler.TerraficFines terraficFine)
        {
            string outputString = string.Empty;

            outputString += $"قبض شماره {Helper.ToPersianNumber(currentPage + 1)} از {Helper.ToPersianNumber(totalPage)}{System.Environment.NewLine}";
            outputString += System.Environment.NewLine;
            outputString += $"شهر: {Helper.ToPersianNumber(terraficFine.City)}{System.Environment.NewLine}";
            outputString += $"تاریخ: {Helper.ToPersianNumber(terraficFine.DateTime)}{System.Environment.NewLine}";
            outputString += $"ثبت توسط: {Helper.ToPersianNumber(terraficFine.Delivery)}{System.Environment.NewLine}";
            outputString += $"مکان: {Helper.ToPersianNumber(terraficFine.Location)}{System.Environment.NewLine}";
            outputString += $"نوع: {Helper.ToPersianNumber(terraficFine.Type)}{System.Environment.NewLine}";
            outputString += $"مبلغ: {Helper.ToPersianNumber(Helper.ToTomanCurrency(terraficFine.Amount))}{System.Environment.NewLine}";
            outputString += System.Environment.NewLine;

            return outputString;
        }
        private void Register(string userInputText = "")
        {
            switch (UserInfo.UserState)
            {
                case UserState.NotRegistered:
                    TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, "برای ثبت نام و استفاده از قبضینو، لطفا شماره موبایل خود را وارد کنید:");

                    UserInfo.UserState = UserState.RequestingActivationCode;
                    DataHandler.SaveUserInfo(UserInfo);
                    break;

                case UserState.RequestingActivationCode:
                    Mobile mobile = new Mobile();
                    mobile.Number = Helper.ToEnglishNumber(userInputText);
                    mobile.IsNumberContentValid();

                    if (!mobile.NumberContentIsValid)
                    {
                        TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, "شماره موبایل وارد شده صحیح نیست، لطفا شماره موبایل خود را وارد کنید:");

                        UserInfo.UserState = UserState.RequestingActivationCode;
                        DataHandler.SaveUserInfo(UserInfo);

                        return;
                    }

                    var webserviceOutput = GhabzinoCoreApi.RequestActivationCode(mobile.InternationalNumber);
                    if (webserviceOutput.Status.Code.ToString() == ConstantStrings.WebserviceStatusSuccess)//RequestActivationCode Successful
                    {
                        UserInfo.Mobile = mobile.InternationalNumber;//???

                        if (webserviceOutput.Parameters.TwoPhaseAuthentication)//Two-Step Activation
                        {
                            TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, "لطفا کد پیامکی را که چند لحظه دیگر به دستتان می رسد، وارد کنید:");

                            UserInfo.UserState = UserState.ActivationCodeSent;
                            DataHandler.SaveUserInfo(UserInfo);
                        }
                        else//One-Step Activation
                        {
                            var webserviceOutput2 = GhabzinoCoreApi.Login(mobile.InternationalNumber, "1111");

                            if (webserviceOutput2.Status.Code == ConstantStrings.WebserviceStatusSuccess)//Login Successful
                            {
                                UserInfo.Token = webserviceOutput2.Parameters.Token;
                                UserInfo.UserState = UserState.Registered;
                                DataHandler.SaveUserInfo(UserInfo);

                                TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.MainMenu, $"ثبت نام با موفقیت انجام شد.{System.Environment.NewLine}{ConstantStrings.MainMenuDescription}");
                            }
                            else//Login Unsuccessful
                            {
                                UserInfo.UserState = UserState.NotRegistered;
                                DataHandler.SaveUserInfo(UserInfo);

                                TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, webserviceOutput2.Status.Description);
                            }
                        }
                    }
                    else//RequestActivationCode Unsuccessful
                    {
                        UserInfo.UserState = UserState.NotRegistered;
                        DataHandler.SaveUserInfo(UserInfo);

                        TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, webserviceOutput.Status.Description);
                    }
                    break;

                case UserState.ActivationCodeSent:
                    var webserviceOutput3 = GhabzinoCoreApi.Login(UserInfo.Mobile, userInputText);

                    if (webserviceOutput3.Status.Code == ConstantStrings.WebserviceStatusSuccess)//Login Successful
                    {
                        UserInfo.Token = webserviceOutput3.Parameters.Token;
                        UserInfo.UserState = UserState.Registered;
                        DataHandler.SaveUserInfo(UserInfo);

                        TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.MainMenu, $"ثبت نام با موفقیت انجام شد.{System.Environment.NewLine}{ConstantStrings.MainMenuDescription}");
                    }
                    else//Login Unsuccessful
                    {
                        UserInfo.UserState = UserState.NotRegistered;
                        DataHandler.SaveUserInfo(UserInfo);

                        TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, webserviceOutput3.Status.Description);
                    }
                    break;

                case UserState.Registered:
                    break;
                default:
                    break;
            }

            if (UserInfo.UserState == UserState.NotRegistered)
            {

            }
            else if (UserInfo.UserState == UserState.RequestingActivationCode)
            {
            }
            else if (UserInfo.UserState == UserState.ActivationCodeSent)
            {

            }
            else
            {
                //ERROR
            }
        }
        private void ShowMainMenu(string message = "")
        {
            TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.MainMenu, message);
        }
        private void WaterBillInquiry(string messageText = "")
        {
            try
            {
                switch (UserInfo.WaterBillInquiryStep)
                {
                    case WaterBillInquiryStep.None:
                        TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, ConstantStrings.WaterBillInquiryNone);
                        UserInfo.GasBillInquiryStep = GasBillInquiryStep.Inquiring;
                        break;

                    case WaterBillInquiryStep.Inquiring:
                        var WebserviceResult = GhabzinoCoreApi.WaterHandler(UserInfo.Token, messageText);
                        if (WebserviceResult.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                        {
                            var formattedBill = FormatBillDetails(WebserviceResult) + System.Environment.NewLine;

                            if (WebserviceResult.Parameters.ValidForPayment)
                            {
                                DataHandler.SavePaymentInfo(UserInfo.Token, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.BillID, PaymentID = WebserviceResult.Parameters.PaymentID } });
                                TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
                                UserInfo.WaterBillInquiryStep = WaterBillInquiryStep.Inqueried;
                            }
                            else
                            {
                                ResetFieldAndStep();
                                ShowMainMenu(formattedBill);//???
                                //popup
                            }
                        }
                        else
                        {
                            ResetFieldAndStep();
                            ShowMainMenu(WebserviceResult.Status.Description);//???
                            //popup
                        }
                        break;

                    case WaterBillInquiryStep.Inqueried:
                        var WebserviceResult2 = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, DataHandler.ReadPaymentInfo(UserInfo.Token));
                        if (WebserviceResult2.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                        {
                            TelegramApi.ShowInlineKeyboard(UserInfo.UserID, KeyboardType.GoToPaymentPageSingle, ConstantStrings.PayInlineButton, WebserviceResult2.Parameters.PaymentLink);
                            TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, ConstantStrings.OrReturnToMainMenu);
                            ResetFieldAndStep();
                        }
                        else
                        {
                            //popup
                            ShowMainMenu(WebserviceResult2.Status.Description);//???
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (System.Exception)
            {
                ResetFieldAndStep();
                //Log
                throw;
            }
        }
        private void GasBillInquiry(string messageText = "")
        {
            try
            {
                switch (UserInfo.GasBillInquiryStep)
                {
                    case GasBillInquiryStep.None:
                        TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, ConstantStrings.GasBillInquiryNone);
                        UserInfo.GasBillInquiryStep = GasBillInquiryStep.Inquiring;
                        break;

                    case GasBillInquiryStep.Inquiring:
                        var WebserviceResult = GhabzinoCoreApi.GasHandler(UserInfo.Token, messageText);
                        if (WebserviceResult.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                        {
                            var formattedBill = FormatBillDetails(WebserviceResult) + System.Environment.NewLine;

                            if (WebserviceResult.Parameters.ValidForPayment)
                            {
                                DataHandler.SavePaymentInfo(UserInfo.Token, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.BillID, PaymentID = WebserviceResult.Parameters.PaymentID } });
                                TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
                                UserInfo.GasBillInquiryStep = GasBillInquiryStep.Inqueried;
                            }
                            else
                            {
                                ResetFieldAndStep();
                                ShowMainMenu(formattedBill);//???
                                //popup
                            }
                        }
                        else
                        {
                            ResetFieldAndStep();
                            ShowMainMenu(WebserviceResult.Status.Description);//???
                            //popup
                        }
                        break;

                    case GasBillInquiryStep.Inqueried:
                        var WebserviceResult2 = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, DataHandler.ReadPaymentInfo(UserInfo.Token));
                        if (WebserviceResult2.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                        {
                            TelegramApi.ShowInlineKeyboard(UserInfo.UserID, KeyboardType.GoToPaymentPageSingle, ConstantStrings.PayInlineButton, WebserviceResult2.Parameters.PaymentLink);
                            TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, ConstantStrings.OrReturnToMainMenu);
                            ResetFieldAndStep();
                        }
                        else
                        {
                            //popup
                            ShowMainMenu(WebserviceResult2.Status.Description);//???
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (System.Exception)
            {
                ResetFieldAndStep();
                //Log
                throw;
            }
        }
        private void ElectricityBillInquiry(string messageText = "")
        {
            try
            {
                switch (UserInfo.ElectricityBillInquiryStep)
                {
                    case ElectricityBillInquiryStep.None:
                        TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, ConstantStrings.ElectricityBillInquiryNone);
                        UserInfo.ElectricityBillInquiryStep = ElectricityBillInquiryStep.Inquiring;
                        break;

                    case ElectricityBillInquiryStep.Inquiring:
                        var WebserviceResult = GhabzinoCoreApi.ElectricityHandler(UserInfo.Token, messageText);
                        if (WebserviceResult.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                        {
                            var formattedBill = FormatBillDetails(WebserviceResult) + System.Environment.NewLine;

                            if (WebserviceResult.Parameters.ValidForPayment)
                            {
                                DataHandler.SavePaymentInfo(UserInfo.Token, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.BillID, PaymentID = WebserviceResult.Parameters.PaymentID } });
                                TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
                                UserInfo.ElectricityBillInquiryStep = ElectricityBillInquiryStep.Inqueried;
                            }
                            else
                            {
                                ResetFieldAndStep();
                                ShowMainMenu(formattedBill);//???
                                //popup
                            }
                        }
                        else
                        {
                            ResetFieldAndStep();
                            ShowMainMenu(WebserviceResult.Status.Description);//???
                            //popup
                        }
                        break;

                    case ElectricityBillInquiryStep.Inqueried:
                        var WebserviceResult2 = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, DataHandler.ReadPaymentInfo(UserInfo.Token));
                        if (WebserviceResult2.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                        {
                            TelegramApi.ShowInlineKeyboard(UserInfo.UserID, KeyboardType.GoToPaymentPageSingle, ConstantStrings.PayInlineButton, WebserviceResult2.Parameters.PaymentLink);
                            TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, ConstantStrings.OrReturnToMainMenu);
                            ResetFieldAndStep();
                        }
                        else
                        {
                            //popup
                            ShowMainMenu(WebserviceResult2.Status.Description);//???
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (System.Exception)
            {
                ResetFieldAndStep();
                //Log
                throw;
            }
        }
        private void MciMobileBillInquiry(string messageText = "")
        {
            try
            {
                switch (UserInfo.MciMobileBillInquiryStep)
                {
                    case MciMobileBillInquiryStep.None:
                        TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, ConstantStrings.MciMobileBillInquiryNone);
                        UserInfo.FixedLineBillInquiryStep = FixedLineBillInquiryStep.Inquiring;
                        break;

                    case MciMobileBillInquiryStep.Inquiring:
                        var WebserviceResult = GhabzinoCoreApi.MciMobileHandler(UserInfo.Token, messageText);
                        if (WebserviceResult.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                        {
                            var formattedBill = FormatBillDetails(WebserviceResult) + System.Environment.NewLine;

                            //var validForPaymentCount = 0;
                            var finalIsValid = WebserviceResult.Parameters.FinalTerm.ValidForPayment;
                            var midIsValid = WebserviceResult.Parameters.MidTerm.ValidForPayment;

                            //if (WebserviceResult.Parameters.FinalTerm.ValidForPayment)
                            //{
                            //    validForPaymentCount++;
                            //}
                            //if (WebserviceResult.Parameters.MidTerm.ValidForPayment)
                            //{
                            //    validForPaymentCount++;
                            //}

                            //var reportNewPaymentInputParams = new ReportNewPaymentInputParams[validForPaymentCount];
                            if (finalIsValid && midIsValid)
                            {
                                DataHandler.SavePaymentInfo(UserInfo.Token, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.FinalTerm.BillID, PaymentID = WebserviceResult.Parameters.FinalTerm.PaymentID }, new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.MidTerm.BillID, PaymentID = WebserviceResult.Parameters.MidTerm.PaymentID }, });
                                TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
                                UserInfo.FixedLineBillInquiryStep = FixedLineBillInquiryStep.Inqueried;
                            }
                            else if (finalIsValid)
                            {
                                DataHandler.SavePaymentInfo(UserInfo.Token, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.FinalTerm.BillID, PaymentID = WebserviceResult.Parameters.FinalTerm.PaymentID } });
                                TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
                                UserInfo.FixedLineBillInquiryStep = FixedLineBillInquiryStep.Inqueried;
                            }
                            else if (midIsValid)
                            {
                                DataHandler.SavePaymentInfo(UserInfo.Token, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.MidTerm.BillID, PaymentID = WebserviceResult.Parameters.MidTerm.PaymentID } });
                                TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
                                UserInfo.FixedLineBillInquiryStep = FixedLineBillInquiryStep.Inqueried;
                            }
                            else
                            {
                                ResetFieldAndStep();
                                ShowMainMenu(formattedBill);//???
                                //popup
                            }
                        }
                        else
                        {
                            ResetFieldAndStep();
                            ShowMainMenu(WebserviceResult.Status.Description);//???
                            //popup
                        }
                        break;

                    case MciMobileBillInquiryStep.Inqueried:
                        var newPaymentInputParams = DataHandler.ReadPaymentInfo(UserInfo.Token);

                        if (newPaymentInputParams.Length == 2)//Showing 2 buttons
                        {
                            var optionalUrls = new string[2];

                            for (int i = 0; i < newPaymentInputParams.Length; i++)
                            {
                                var paymentInputParam = newPaymentInputParams[i];
                                var WebserviceResult2 = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, new ReportNewPaymentInputParams[] { paymentInputParam });
                                if (WebserviceResult2.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                                {
                                    optionalUrls[i] = WebserviceResult2.Parameters.PaymentLink;
                                }
                                else
                                {
                                    //popup
                                    ShowMainMenu(WebserviceResult2.Status.Description);//???
                                    break;
                                }
                            }
                            TelegramApi.ShowInlineKeyboard(UserInfo.UserID, KeyboardType.GoToPaymentPageForPhones, ConstantStrings.PayInlineButtons, optionalUrls);
                            TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, ConstantStrings.OrReturnToMainMenu);
                            ResetFieldAndStep();
                        }
                        else if (newPaymentInputParams.Length == 1)//Showing 1 button
                        {
                            var WebserviceResult2 = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, newPaymentInputParams);

                            if (WebserviceResult2.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                            {
                                TelegramApi.ShowInlineKeyboard(UserInfo.UserID, KeyboardType.GoToPaymentPageSingle, ConstantStrings.PayInlineButton, WebserviceResult2.Parameters.PaymentLink);
                                TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, ConstantStrings.OrReturnToMainMenu);
                                ResetFieldAndStep();
                            }
                            else
                            {
                                //popup
                                ShowMainMenu(WebserviceResult2.Status.Description);//???
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (System.Exception)
            {
                ResetFieldAndStep();
                //Log
                throw;
            }

        }
        private void FixedLineBillInquiry(string messageText = "")
        {
            try
            {
                switch (UserInfo.FixedLineBillInquiryStep)
                {
                    case FixedLineBillInquiryStep.None:
                        TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, ConstantStrings.FixedLineBillInquiryNone);
                        UserInfo.FixedLineBillInquiryStep = FixedLineBillInquiryStep.Inquiring;
                        break;

                    case FixedLineBillInquiryStep.Inquiring:
                        var WebserviceResult = GhabzinoCoreApi.FixedLineHandler(UserInfo.Token, messageText);
                        if (WebserviceResult.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                        {
                            var formattedBill = FormatBillDetails(WebserviceResult) + System.Environment.NewLine;

                            //var validForPaymentCount = 0;
                            var finalIsValid = WebserviceResult.Parameters.FinalTerm.ValidForPayment;
                            var midIsValid = WebserviceResult.Parameters.MidTerm.ValidForPayment;

                            //if (WebserviceResult.Parameters.FinalTerm.ValidForPayment)
                            //{
                            //    validForPaymentCount++;
                            //}
                            //if (WebserviceResult.Parameters.MidTerm.ValidForPayment)
                            //{
                            //    validForPaymentCount++;
                            //}

                            //var reportNewPaymentInputParams = new ReportNewPaymentInputParams[validForPaymentCount];
                            if (finalIsValid && midIsValid)
                            {
                                DataHandler.SavePaymentInfo(UserInfo.Token, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.FinalTerm.BillID, PaymentID = WebserviceResult.Parameters.FinalTerm.PaymentID }, new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.MidTerm.BillID, PaymentID = WebserviceResult.Parameters.MidTerm.PaymentID }, });
                                TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
                                UserInfo.FixedLineBillInquiryStep = FixedLineBillInquiryStep.Inqueried;
                            }
                            else if (finalIsValid)
                            {
                                DataHandler.SavePaymentInfo(UserInfo.Token, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.FinalTerm.BillID, PaymentID = WebserviceResult.Parameters.FinalTerm.PaymentID } });
                                TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
                                UserInfo.FixedLineBillInquiryStep = FixedLineBillInquiryStep.Inqueried;
                            }
                            else if (midIsValid)
                            {
                                DataHandler.SavePaymentInfo(UserInfo.Token, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.MidTerm.BillID, PaymentID = WebserviceResult.Parameters.MidTerm.PaymentID } });
                                TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
                                UserInfo.FixedLineBillInquiryStep = FixedLineBillInquiryStep.Inqueried;
                            }
                            else
                            {
                                ResetFieldAndStep();
                                ShowMainMenu(formattedBill);//???
                                //popup
                            }
                        }
                        else
                        {
                            ResetFieldAndStep();
                            ShowMainMenu(WebserviceResult.Status.Description);//???
                            //popup
                        }
                        break;

                    case FixedLineBillInquiryStep.Inqueried:
                        var newPaymentInputParams = DataHandler.ReadPaymentInfo(UserInfo.Token);

                        if (newPaymentInputParams.Length == 2)//Showing 2 buttons
                        {
                            var optionalUrls = new string[2];

                            for (int i = 0; i < newPaymentInputParams.Length; i++)
                            {
                                var paymentInputParam = newPaymentInputParams[i];
                                var WebserviceResult2 = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, new ReportNewPaymentInputParams[] { paymentInputParam });
                                if (WebserviceResult2.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                                {
                                    optionalUrls[i] = WebserviceResult2.Parameters.PaymentLink;
                                }
                                else
                                {
                                    //popup
                                    ShowMainMenu(WebserviceResult2.Status.Description);//???
                                    break;
                                }
                            }
                            TelegramApi.ShowInlineKeyboard(UserInfo.UserID, KeyboardType.GoToPaymentPageForPhones, ConstantStrings.PayInlineButtons, optionalUrls);
                            TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, ConstantStrings.OrReturnToMainMenu);
                            ResetFieldAndStep();
                        }
                        else if (newPaymentInputParams.Length == 1)//Showing 1 button
                        {
                            var WebserviceResult2 = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, newPaymentInputParams);

                            if (WebserviceResult2.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                            {
                                TelegramApi.ShowInlineKeyboard(UserInfo.UserID, KeyboardType.GoToPaymentPageSingle, ConstantStrings.PayInlineButton, WebserviceResult2.Parameters.PaymentLink);
                                TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, ConstantStrings.OrReturnToMainMenu);
                                ResetFieldAndStep();
                            }
                            else
                            {
                                //popup
                                ShowMainMenu(WebserviceResult2.Status.Description);//???
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (System.Exception)
            {
                ResetFieldAndStep();
                //Log
                throw;
            }
        }
        private void TrafficFinesInquiry(string messageText = "")
        {
            try
            {
                switch (UserInfo.TrafficFinesInquiryStep)
                {
                    case TrafficFinesInquiryStep.None:
                        TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, ConstantStrings.TrafficFinesInquiryNone);
                        UserInfo.TrafficFinesInquiryStep = TrafficFinesInquiryStep.Inquiring;
                        break;

                    case TrafficFinesInquiryStep.Inquiring:
                        var WebserviceResult = GhabzinoCoreApi.TrafficFinesHandler(UserInfo.Token, messageText);
                        if (WebserviceResult.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                        {
                            var formattedBill = FormatBillDetails(WebserviceResult) + System.Environment.NewLine;

                            var validForPaymentCount = WebserviceResult.Parameters.TotalValidForPaymentCount;
                            if (validForPaymentCount > 0)
                            {
                                ReportNewPaymentInputParams[] reportNewPaymentInputParams = new ReportNewPaymentInputParams[validForPaymentCount];
                                DataHandler.TerraficFines[] terraficFines = new DataHandler.TerraficFines[validForPaymentCount];
                                for (int i = 0, j = 0; i < WebserviceResult.Parameters.Details.Length; i++)
                                {
                                    var item = WebserviceResult.Parameters.Details[i];
                                    if (item.ValidForPayment)
                                    {
                                        reportNewPaymentInputParams[j] = new ReportNewPaymentInputParams() { BillID = item.BillID, PaymentID = item.PaymentID };
                                        terraficFines[j] = new DataHandler.TerraficFines() { City = item.City, DateTime = item.DateTime, Delivery = item.Delivery, Location = item.Location, Type = item.Type, Amount = item.Amount };
                                        j++;
                                    }
                                }
                                DataHandler.SavePaymentInfo(UserInfo.Token, reportNewPaymentInputParams);//مشخصات پرداخت
                                DataHandler.SaveTerraficFinesInfo(UserInfo.Token, terraficFines);//مشخصات خلافی
                                DataHandler.SaveTerraficFinesPage(UserInfo.Token, 0);//صفحه
                                TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ChooseToAddPayment, formattedBill);
                                UserInfo.TrafficFinesInquiryStep = TrafficFinesInquiryStep.Inqueried;
                            }
                            else
                            {
                                ResetFieldAndStep();
                                ShowMainMenu(formattedBill);//???
                                //popup
                            }
                        }
                        else
                        {
                            ResetFieldAndStep();
                            ShowMainMenu(WebserviceResult.Status.Description);//???
                            //popup
                        }
                        break;

                    case TrafficFinesInquiryStep.Inqueried:
                        var allPagesPaymentInputParams = DataHandler.ReadPaymentInfo(UserInfo.Token);
                        var allPagesCount = allPagesPaymentInputParams.Length;
                        var currentPageNumber = DataHandler.ReadTerraficFinesPage(UserInfo.Token);
                        var selectedPagesNumbersArray = DataHandler.ReadSelectedBillsInfo(UserInfo.Token);
                        //====================================================================
                        ReportNewPaymentInputParams[] selectedPagesPaymentInputParams = new ReportNewPaymentInputParams[selectedPagesNumbersArray.Length];
                        for (int i = 0; i < selectedPagesNumbersArray.Length; i++)
                        {
                            selectedPagesPaymentInputParams[i] = allPagesPaymentInputParams[selectedPagesNumbersArray[i]];
                        }

                        ReportNewPaymentOutput webserviceResult2 = null;
                        if (selectedPagesNumbersArray.Length > 0)
                        {
                            webserviceResult2 = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, selectedPagesPaymentInputParams);
                        }
                        //==========================================================================
                        var currentPageDetail = DataHandler.ReadTerraficFinesInfo(UserInfo.Token, currentPageNumber)[0];
                        var FormattedTerraficFines = FormatTerraficFinesDetails(currentPageNumber, allPagesCount, currentPageDetail);
                        //==========================================================================

                        if (messageText == ConstantStrings.Next)
                        {
                            if (currentPageNumber >= allPagesCount)
                            {
                                TelegramApi.AnswerCallback(UserInfo.UserID.ToString(), "همکنون در صفحه آخر هستید.");
                            }
                            else
                            {
                                DataHandler.SaveTerraficFinesPage(UserInfo.Token, ++currentPageNumber);
                            }
                        }
                        else if (messageText == ConstantStrings.Previous)
                        {
                            if (currentPageNumber <= 0)
                            {
                                TelegramApi.AnswerCallback(UserInfo.UserID.ToString(), "همکنون در صفحه اول هستید.");
                            }
                            else
                            {
                                DataHandler.SaveTerraficFinesPage(UserInfo.Token, --currentPageNumber);
                            }
                        }
                        else if (messageText == ConstantStrings.AddAllToPaymentList)
                        {
                            int[] allPages = new int[allPagesCount];
                            for (int i = 0; i < allPagesCount; i++)
                            {
                                allPages[i] = i;
                            }
                            DataHandler.SaveSelectedBillsInfo(UserInfo.Token, allPages);
                        }
                        else if (messageText == ConstantStrings.AddToPaymentList)
                        {
                            if (selectedPagesNumbersArray.Contains(currentPageNumber))
                            {
                                TelegramApi.AnswerCallback(UserInfo.UserID.ToString(), "همکنون در لیست پرداخت است.");
                            }
                            else
                            {
                                selectedPagesNumbersArray = selectedPagesNumbersArray.Concat(new int[] { currentPageNumber }).ToArray();
                                DataHandler.SaveSelectedBillsInfo(UserInfo.Token, selectedPagesNumbersArray);
                                TelegramApi.AnswerCallback(UserInfo.UserID.ToString(), "به لیست اضافه شد.");
                            }
                        }
                        else if (messageText == ConstantStrings.RemoveFromPaymentList)
                        {
                            if (selectedPagesNumbersArray.Contains(currentPageNumber))
                            {
                                selectedPagesNumbersArray = selectedPagesNumbersArray.RemoveAt(Array.IndexOf(selectedPagesNumbersArray, currentPageNumber));
                                DataHandler.SaveSelectedBillsInfo(UserInfo.Token, selectedPagesNumbersArray);
                                TelegramApi.AnswerCallback(UserInfo.UserID.ToString(), "از لیست حذف شد.");
                            }
                            else
                            {
                                TelegramApi.AnswerCallback(UserInfo.UserID.ToString(), "همکنون در لیست پرداخت نیست.");
                            }
                        }
                        else if (messageText == ConstantStrings.payBill)
                        {
                            if (selectedPagesNumbersArray.Length == 0)
                            {
                                TelegramApi.AnswerCallback(UserInfo.UserID.ToString(), "حداقل یک قبض را انتخاب کنید.");
                            }
                            else if (webserviceResult2?.Status?.Code != ConstantStrings.WebserviceStatusSuccess || string.IsNullOrEmpty(webserviceResult2?.Parameters?.PaymentLink))
                            {
                                TelegramApi.AnswerCallback(UserInfo.UserID.ToString(), webserviceResult2.Status.Description);
                                //log
                            }
                            else
                            {
                                TelegramApi.AnswerCallback(UserInfo.UserID.ToString(), "در حال انتقال به درگاه بانک...");
                                ResetFieldAndStep();
                            }
                        }
                        else
                        {
                            //popup
                            ShowMainMenu(webserviceResult2.Status.Description);//???
                        }

                        TelegramApi.ShowInlineKeyboard(UserInfo.UserID, KeyboardType.ChooseToAddPayment, FormattedTerraficFines, webserviceResult2?.Parameters?.PaymentLink);
                        TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, "یا به صفحه اصلی باز گردید.");
                        break;

                    default:
                        break;
                }
            }
            catch (Exception)
            {
                ResetFieldAndStep();
                //Log
                throw;
            }
        }
        private void Bill(string messageText = "")
        {
            switch (UserInfo.BillStep)
            {
                case BillStep.None:
                    TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, "لطفا شناسه قبض را وارد نمایید:");
                    UserInfo.BillStep = BillStep.PaymentId;
                    break;

                case BillStep.PaymentId:
                    long billId = 0;
                    if (!long.TryParse(messageText.ToEnglishNumber(), out billId))
                    {
                        ShowMainMenu("شناسه قبض معتبر نیست");
                    }
                    else
                    {
                        DataHandler.SavePaymentInfo(UserInfo.Token, new ReportNewPaymentInputParams { BillID = messageText });
                        TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, "لطفا شناسه پرداخت را وارد نمایید:");
                        UserInfo.BillStep = BillStep.Inquiring;
                    }
                    break;

                case BillStep.Inquiring:
                    long paymentId = 0;
                    if (!long.TryParse(messageText.ToEnglishNumber(), out paymentId))
                    {
                        ShowMainMenu("شناسه پرداخت معتبر نیست");
                    }
                    else
                    {
                        var webserviceResult = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, new ReportNewPaymentInputParams { BillID = DataHandler.ReadPaymentInfo(UserInfo.Token)[0].BillID, PaymentID = messageText });
                        if (webserviceResult.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                        {
                            var formattedBill = FormatBillDetails(webserviceResult) + Environment.NewLine;//???

                            if (webserviceResult.Parameters.Bills[0].ValidForPayment)
                            {
                                DataHandler.SavePaymentInfo(UserInfo.Token, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = webserviceResult.Parameters.Bills[0].BillID, PaymentID = webserviceResult.Parameters.Bills[0].PaymentID } });
                                TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
                                UserInfo.BillStep = BillStep.Inqueried;
                            }
                            else
                            {
                                ResetFieldAndStep();
                                ShowMainMenu(formattedBill);//???
                                //popup
                            }
                        }
                        else
                        {
                            ResetFieldAndStep();
                            ShowMainMenu(webserviceResult.Status.Description);//???
                            //popup
                        }
                    }
                    break;

                case BillStep.Inqueried:
                    var webserviceResult2 = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, DataHandler.ReadPaymentInfo(UserInfo.Token));
                    if (webserviceResult2.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                    {
                        TelegramApi.ShowInlineKeyboard(UserInfo.UserID, KeyboardType.GoToPaymentPageSingle, ConstantStrings.PayInlineButton, webserviceResult2.Parameters.PaymentLink);
                        TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, ConstantStrings.OrReturnToMainMenu);
                        ResetFieldAndStep();
                    }
                    else
                    {
                        //popup
                        ShowMainMenu(webserviceResult2.Status.Description);//???
                    }
                    break;

                default:
                    break;
            }
        }
        private void History(string messageText = "")
        {
            switch (UserInfo.HistoryStep)
            {
                case HistoryStep.None:
                    TelegramApi.ShowKeyboard(UserInfo.UserID, KeyboardType.History, "مایلید سوابق کدام پرداختی ها را ببینید:");
                    break;
                case HistoryStep.FieldDetermined:
                    string strType = null;
                    if (messageText == ProjectValues.All)
                    {

                    }
                    else if (messageText == ProjectValues.All)
                    {

                    }
                    else if (messageText == ProjectValues.WaterBillInquiry)
                    {

                    }
                    else if (messageText == ProjectValues.ElectricityBillInquiry)
                    {

                    }
                    else if (messageText == ProjectValues.GasBillInquiry)
                    {

                    }
                    else if (messageText == ProjectValues.MciMobileBillInquiry)
                    {

                    }
                    else if (messageText == ProjectValues.FixedLineBillInquiry)
                    {

                    }
                    else if (messageText == ProjectValues.TrafficFinesInquery)
                    {

                    }
                    //call webservice and show results
                    break;
                default:
                    break;
            }
        }
        private void ResetFieldAndStep()
        {
            UserInfo.UserField = UserField.None;
            UserInfo.WaterBillInquiryStep = WaterBillInquiryStep.None;
            UserInfo.GasBillInquiryStep = GasBillInquiryStep.None;
            UserInfo.ElectricityBillInquiryStep = ElectricityBillInquiryStep.None;
            UserInfo.MciMobileBillInquiryStep = MciMobileBillInquiryStep.None;
            UserInfo.FixedLineBillInquiryStep = FixedLineBillInquiryStep.None;
            UserInfo.TrafficFinesInquiryStep = TrafficFinesInquiryStep.None;
            DataHandler.SaveUserInfo(UserInfo);
        }
    }
}
