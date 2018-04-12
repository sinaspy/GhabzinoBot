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

            try
            {
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
                    Log.Info("User is not registered", sw.Elapsed.TotalMilliseconds);//???
                    Register(messageText);
                    return;
                }

                if (UserInfo.UserField == UserField.History)
                {
                    History(messageText);
                }
                else
                {
                    switch (messageText.ToLower())
                    {
                        case ConstantStrings.start:
                        case ConstantStrings.returnToMainMenu:
                            UserInfo.UserField = UserField.None;
                            TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.MainMenu, ConstantStrings.MainMenuDescription);
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
                            //
                            break;

                        case ConstantStrings.history:
                            UserInfo.UserField = UserField.History;
                            UserInfo.HistoryStep = HistoryStep.None;
                            History();
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
                }
                DataHandler.SaveUserInfo(UserInfo);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
            }
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
                    outputString += $"مبلغ: {Helper.ToPersianNumber(Helper.ToTomanCurrency(waterbillObj.Parameters.Amount))} تومان{Environment.NewLine}";
                    outputString += $"نام: {waterbillObj.Parameters.FullName}{Environment.NewLine}";
                    outputString += $"شناسه قبض: {Helper.ToPersianNumber(waterbillObj.Parameters.BillID)}{Environment.NewLine}";
                    outputString += $"شناسه پرداخت: {Helper.ToPersianNumber(waterbillObj.Parameters.PaymentID)}{Environment.NewLine}";
                    outputString += $"مهلت پرداخت: {Helper.ToPersianNumber(waterbillObj.Parameters.PaymentDate)}{Environment.NewLine}";
                    outputString += $"قرائت فعلی: {Helper.ToPersianNumber(waterbillObj.Parameters.PreviousDate)}{Environment.NewLine}";
                    outputString += $"قرائت قبلی: {Helper.ToPersianNumber(waterbillObj.Parameters.CurrentDate)}{Environment.NewLine}";
                    outputString += $"آدرس: {Helper.ToPersianNumber(waterbillObj.Parameters.Address)}{Environment.NewLine}";
                    outputString += Environment.NewLine;
                    //if (!waterbillObj.Parameters.ValidForPayment)
                    //{
                    //    outputString += "قبلا پرداخت شده است.";
                    //}
                    break;

                case UserField.GasBillInquiry:
                    GasBillInquiryOutput gasBillObj = (GasBillInquiryOutput)inputObj;
                    outputString += $"مبلغ: {Helper.ToPersianNumber(Helper.ToTomanCurrency(gasBillObj.Parameters.Amount))} تومان{Environment.NewLine}";
                    outputString += $"نام: {gasBillObj.Parameters.FullName}{Environment.NewLine}";
                    outputString += $"شناسه قبض: {Helper.ToPersianNumber(gasBillObj.Parameters.BillID)}{Environment.NewLine}";
                    outputString += $"شناسه پرداخت: {Helper.ToPersianNumber(gasBillObj.Parameters.PaymentID)}{Environment.NewLine}";
                    outputString += $"مهلت پرداخت: {Helper.ToPersianNumber(gasBillObj.Parameters.PaymentDate)}{Environment.NewLine}";
                    outputString += $"قرائت فعلی: {Helper.ToPersianNumber(gasBillObj.Parameters.PreviousDate)}{Environment.NewLine}";
                    outputString += $"قرائت قبلی: {Helper.ToPersianNumber(gasBillObj.Parameters.CurrentDate)}{Environment.NewLine}";
                    outputString += $"آدرس: {Helper.ToPersianNumber(gasBillObj.Parameters.Address)}{Environment.NewLine}";
                    outputString += Environment.NewLine;
                    //if (!gasBillObj.Parameters.ValidForPayment)
                    //{
                    //    outputString += "قبلا پرداخت شده است.";
                    //}
                    break;

                case UserField.ElectricityBillInquiry:
                    ElectricityBillInquiryOutput electricityBillObj = (ElectricityBillInquiryOutput)inputObj;
                    outputString += $"مبلغ: {Helper.ToPersianNumber(Helper.ToTomanCurrency(electricityBillObj.Parameters.Amount))} تومان{Environment.NewLine}";
                    outputString += $"نام: {electricityBillObj.Parameters.FullName}{Environment.NewLine}";
                    outputString += $"شناسه قبض: {Helper.ToPersianNumber(electricityBillObj.Parameters.BillID)}{Environment.NewLine}";
                    outputString += $"شناسه پرداخت: {Helper.ToPersianNumber(electricityBillObj.Parameters.PaymentID)}{Environment.NewLine}";
                    outputString += $"مهلت پرداخت: {Helper.ToPersianNumber(electricityBillObj.Parameters.PaymentDate)}{Environment.NewLine}";
                    outputString += $"قرائت فعلی: {Helper.ToPersianNumber(electricityBillObj.Parameters.PreviousDate)}{Environment.NewLine}";
                    outputString += $"قرائت قبلی: {Helper.ToPersianNumber(electricityBillObj.Parameters.CurrentDate)}{Environment.NewLine}";
                    outputString += $"آدرس: {Helper.ToPersianNumber(electricityBillObj.Parameters.Address)}{Environment.NewLine}";
                    outputString += Environment.NewLine;
                    //if (!electricityBillObj.Parameters.ValidForPayment)
                    //{
                    //    outputString += "قبلا پرداخت شده است.";
                    //}
                    break;

                case UserField.MciMobileBillInquiry:
                    MCIMobileBillInquiryOutput mciMobileBillObj = (MCIMobileBillInquiryOutput)inputObj;
                    outputString += $"پایان دوره{Environment.NewLine}";
                    if (mciMobileBillObj.Parameters.FinalTerm.ValidForPayment)
                    {
                        outputString += $"{Helper.ToPersianNumber(Helper.ToTomanCurrency(mciMobileBillObj.Parameters.FinalTerm.Amount))} تومان{Environment.NewLine}";
                        outputString += $"شناسه قبض: {Helper.ToPersianNumber(mciMobileBillObj.Parameters.FinalTerm.BillID)}{Environment.NewLine}";
                        outputString += $"شناسه پرداخت: {Helper.ToPersianNumber(mciMobileBillObj.Parameters.FinalTerm.PaymentID)}{Environment.NewLine}";
                    }
                    else
                    {
                        outputString += "قبلا پرداخت شده است." + Environment.NewLine;
                    }
                    outputString += Environment.NewLine;
                    outputString += $"میان دوره{Environment.NewLine}";
                    if (mciMobileBillObj.Parameters.MidTerm.ValidForPayment)
                    {
                        outputString += $"{Helper.ToPersianNumber(Helper.ToTomanCurrency(mciMobileBillObj.Parameters.MidTerm.Amount))} تومان{Environment.NewLine}";
                        outputString += $"شناسه قبض: {Helper.ToPersianNumber(mciMobileBillObj.Parameters.MidTerm.BillID)}{Environment.NewLine}";
                        outputString += $"شناسه پرداخت: {Helper.ToPersianNumber(mciMobileBillObj.Parameters.MidTerm.PaymentID)}{Environment.NewLine}";
                    }
                    else
                    {
                        outputString += "قبلا پرداخت شده است." + Environment.NewLine;
                    }
                    outputString += Environment.NewLine;
                    break;

                case UserField.FixedLineBillInquiry:
                    FixedLineBillInquiryOutput fixedLineBillObj = (FixedLineBillInquiryOutput)inputObj;
                    outputString += $"پایان دوره{Environment.NewLine}";
                    if (fixedLineBillObj.Parameters.FinalTerm.ValidForPayment)
                    {
                        outputString += $"{Helper.ToPersianNumber(Helper.ToTomanCurrency(fixedLineBillObj.Parameters.FinalTerm.Amount))} تومان{Environment.NewLine}";
                        outputString += $"شناسه قبض: {Helper.ToPersianNumber(fixedLineBillObj.Parameters.FinalTerm.BillID)}{Environment.NewLine}";
                        outputString += $"شناسه پرداخت: {Helper.ToPersianNumber(fixedLineBillObj.Parameters.FinalTerm.PaymentID)}{Environment.NewLine}";
                    }
                    else
                    {
                        outputString += "قبلا پرداخت شده است." + Environment.NewLine;
                    }
                    outputString += Environment.NewLine;
                    outputString += $"میان دوره{Environment.NewLine}";
                    if (fixedLineBillObj.Parameters.MidTerm.ValidForPayment)
                    {
                        outputString += $"{Helper.ToPersianNumber(Helper.ToTomanCurrency(fixedLineBillObj.Parameters.MidTerm.Amount))} تومان{Environment.NewLine}";
                        outputString += $"شناسه قبض: {Helper.ToPersianNumber(fixedLineBillObj.Parameters.MidTerm.BillID)}{Environment.NewLine}";
                        outputString += $"شناسه پرداخت: {Helper.ToPersianNumber(fixedLineBillObj.Parameters.MidTerm.PaymentID)}{Environment.NewLine}";
                    }
                    else
                    {
                        outputString += "قبلا پرداخت شده است." + Environment.NewLine;
                    }
                    outputString += Environment.NewLine;
                    break;

                case UserField.TrafficFinesInquery:
                    TrafficFinesInquiryOutput TrafficFinesObj = (TrafficFinesInquiryOutput)inputObj;
                    outputString += $"پلاک: {Helper.ToPersianNumber(TrafficFinesObj.Parameters.PlateNumber)}{Environment.NewLine}";
                    outputString += $"بارکد: {Helper.ToPersianNumber(TrafficFinesObj.Parameters.Inquiry.Value)}{Environment.NewLine}";
                    outputString += $"تعداد کل جریمه های قابل پرداخت: {Helper.ToPersianNumber(TrafficFinesObj.Parameters.TotalValidForPaymentCount.ToString())}{Environment.NewLine}";
                    outputString += $"مبلغ کل: {Helper.ToPersianNumber(Helper.ToTomanCurrency(TrafficFinesObj.Parameters.TotalAmount))} تومان{Environment.NewLine}";
                    outputString += Environment.NewLine;
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
        private string FormatPaymentHistoryDetails(GetEndUserPaymentHistoryDetailOutput inputObj, string messageText)
        {
            string outputString = string.Empty;

            outputString += $"تعداد کل قبوض پرداخت شده ";
            if (!string.Equals(messageText, ProjectValues.All, StringComparison.OrdinalIgnoreCase))
            {
                outputString += $"از نوع {messageText}";
            }
            outputString += $": {inputObj.Parameters.TotalBillsCount}{Environment.NewLine}";
            outputString += $"کل مبلغ پرداخت شده: {Helper.ToPersianNumber(Helper.ToTomanCurrency(inputObj.Parameters.TotalAmount))} تومان{Environment.NewLine}";
            outputString += Environment.NewLine;

            return outputString;
        }
        private void Register(string userInputText = "")
        {
            switch (UserInfo.UserState)
            {
                case UserState.NotRegistered:
                    TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, "برای ثبت نام و استفاده از قبضینو، لطفا شماره موبایل خود را وارد کنید:");

                    UserInfo.UserState = UserState.RequestingActivationCode;
                    DataHandler.SaveUserInfo(UserInfo);
                    break;

                case UserState.RequestingActivationCode:
                    Mobile mobile = new Mobile();
                    mobile.Number = Helper.ToEnglishNumber(userInputText);
                    mobile.IsNumberContentValid();

                    if (!mobile.NumberContentIsValid)
                    {
                        TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, "شماره موبایل وارد شده صحیح نیست، لطفا شماره موبایل خود را وارد کنید:");

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
                            TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, "لطفا کد پیامکی را که چند لحظه دیگر به دستتان می رسد، وارد کنید:");

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

                                TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.MainMenu, $"ثبت نام با موفقیت انجام شد.{System.Environment.NewLine}{ConstantStrings.MainMenuDescription}");
                            }
                            else//Login Unsuccessful
                            {
                                UserInfo.UserState = UserState.NotRegistered;
                                DataHandler.SaveUserInfo(UserInfo);

                                TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, webserviceOutput2.Status.Description);
                            }
                        }
                    }
                    else//RequestActivationCode Unsuccessful
                    {
                        UserInfo.UserState = UserState.NotRegistered;
                        DataHandler.SaveUserInfo(UserInfo);

                        TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, webserviceOutput.Status.Description);
                    }
                    break;

                case UserState.ActivationCodeSent:
                    var webserviceOutput3 = GhabzinoCoreApi.Login(UserInfo.Mobile, userInputText);

                    if (webserviceOutput3.Status.Code == ConstantStrings.WebserviceStatusSuccess)//Login Successful
                    {
                        UserInfo.Token = webserviceOutput3.Parameters.Token;
                        UserInfo.UserState = UserState.Registered;
                        DataHandler.SaveUserInfo(UserInfo);

                        TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.MainMenu, $"ثبت نام با موفقیت انجام شد.{System.Environment.NewLine}{ConstantStrings.MainMenuDescription}");
                    }
                    else//Login Unsuccessful
                    {
                        UserInfo.UserState = UserState.NotRegistered;
                        DataHandler.SaveUserInfo(UserInfo);

                        TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, webserviceOutput3.Status.Description);
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
            TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.MainMenu, message);
        }
        private void WaterBillInquiry(string messageText = "")
        {

            switch (UserInfo.WaterBillInquiryStep)
            {
                case WaterBillInquiryStep.None:
                    TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, ConstantStrings.WaterBillInquiryNone);
                    UserInfo.WaterBillInquiryStep = WaterBillInquiryStep.Inquiring;
                    break;

                case WaterBillInquiryStep.Inquiring:
                    var WebserviceResult = GhabzinoCoreApi.WaterHandler(UserInfo.Token, messageText);
                    if (WebserviceResult.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                    {
                        var formattedBill = FormatBillDetails(WebserviceResult) + System.Environment.NewLine;

                        if (WebserviceResult.Parameters.ValidForPayment)
                        {
                            DataHandler.SavePaymentInfo(UserInfo.UserId, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.BillID, PaymentID = WebserviceResult.Parameters.PaymentID } });
                            TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.AddPayment, formattedBill);
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
                    var WebserviceResult2 = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, DataHandler.ReadPaymentInfo(UserInfo.UserId));
                    if (WebserviceResult2.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                    {
                        TelegramApi.ShowInlineKeyboard(UserInfo.UserId, KeyboardType.GoToPaymentPageSingle, ConstantStrings.PayInlineButton, WebserviceResult2.Parameters.PaymentLink);
                        TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, ConstantStrings.OrReturnToMainMenu);
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
        private void GasBillInquiry(string messageText = "")
        {
            switch (UserInfo.GasBillInquiryStep)
            {
                case GasBillInquiryStep.None:
                    TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, ConstantStrings.GasBillInquiryNone);
                    UserInfo.GasBillInquiryStep = GasBillInquiryStep.Inquiring;
                    break;

                case GasBillInquiryStep.Inquiring:
                    var WebserviceResult = GhabzinoCoreApi.GasHandler(UserInfo.Token, messageText);
                    if (WebserviceResult.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                    {
                        var formattedBill = FormatBillDetails(WebserviceResult) + System.Environment.NewLine;

                        if (WebserviceResult.Parameters.ValidForPayment)
                        {
                            DataHandler.SavePaymentInfo(UserInfo.UserId, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.BillID, PaymentID = WebserviceResult.Parameters.PaymentID } });
                            TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.AddPayment, formattedBill);
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
                    var WebserviceResult2 = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, DataHandler.ReadPaymentInfo(UserInfo.UserId));
                    if (WebserviceResult2.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                    {
                        TelegramApi.ShowInlineKeyboard(UserInfo.UserId, KeyboardType.GoToPaymentPageSingle, ConstantStrings.PayInlineButton, WebserviceResult2.Parameters.PaymentLink);
                        TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, ConstantStrings.OrReturnToMainMenu);
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
        private void ElectricityBillInquiry(string messageText = "")
        {
            switch (UserInfo.ElectricityBillInquiryStep)
            {
                case ElectricityBillInquiryStep.None:
                    TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, ConstantStrings.ElectricityBillInquiryNone);
                    UserInfo.ElectricityBillInquiryStep = ElectricityBillInquiryStep.Inquiring;
                    break;

                case ElectricityBillInquiryStep.Inquiring:
                    var WebserviceResult = GhabzinoCoreApi.ElectricityHandler(UserInfo.Token, messageText);
                    if (WebserviceResult.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                    {
                        var formattedBill = FormatBillDetails(WebserviceResult) + System.Environment.NewLine;

                        if (WebserviceResult.Parameters.ValidForPayment)
                        {
                            DataHandler.SavePaymentInfo(UserInfo.UserId, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.BillID, PaymentID = WebserviceResult.Parameters.PaymentID } });
                            TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.AddPayment, formattedBill);
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
                    var WebserviceResult2 = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, DataHandler.ReadPaymentInfo(UserInfo.UserId));
                    if (WebserviceResult2.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                    {
                        TelegramApi.ShowInlineKeyboard(UserInfo.UserId, KeyboardType.GoToPaymentPageSingle, ConstantStrings.PayInlineButton, WebserviceResult2.Parameters.PaymentLink);
                        TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, ConstantStrings.OrReturnToMainMenu);
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
        private void MciMobileBillInquiry(string messageText = "")
        {
            switch (UserInfo.MciMobileBillInquiryStep)
            {
                case MciMobileBillInquiryStep.None:
                    TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, ConstantStrings.MciMobileBillInquiryNone);
                    UserInfo.MciMobileBillInquiryStep = MciMobileBillInquiryStep.Inquiring;
                    break;

                case MciMobileBillInquiryStep.Inquiring:
                    var WebserviceResult = GhabzinoCoreApi.MciMobileHandler(UserInfo.Token, messageText);
                    if (WebserviceResult.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                    {
                        var formattedBill = FormatBillDetails(WebserviceResult) + System.Environment.NewLine;

                        var finalIsValid = WebserviceResult.Parameters.FinalTerm.ValidForPayment;
                        var midIsValid = WebserviceResult.Parameters.MidTerm.ValidForPayment;

                        if (finalIsValid && midIsValid)
                        {
                            DataHandler.SavePaymentInfo(UserInfo.UserId, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.FinalTerm.BillID, PaymentID = WebserviceResult.Parameters.FinalTerm.PaymentID }, new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.MidTerm.BillID, PaymentID = WebserviceResult.Parameters.MidTerm.PaymentID }, });
                            TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.AddPayment, formattedBill);
                            UserInfo.MciMobileBillInquiryStep = MciMobileBillInquiryStep.Inqueried;
                        }
                        else if (finalIsValid)
                        {
                            DataHandler.SavePaymentInfo(UserInfo.UserId, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.FinalTerm.BillID, PaymentID = WebserviceResult.Parameters.FinalTerm.PaymentID } });
                            TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.AddPayment, formattedBill);
                            UserInfo.MciMobileBillInquiryStep = MciMobileBillInquiryStep.Inqueried;
                        }
                        else if (midIsValid)
                        {
                            DataHandler.SavePaymentInfo(UserInfo.UserId, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.MidTerm.BillID, PaymentID = WebserviceResult.Parameters.MidTerm.PaymentID } });
                            TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.AddPayment, formattedBill);
                            UserInfo.MciMobileBillInquiryStep = MciMobileBillInquiryStep.Inqueried;
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
                    var newPaymentInputParams = DataHandler.ReadPaymentInfo(UserInfo.UserId);

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
                        TelegramApi.ShowInlineKeyboard(UserInfo.UserId, KeyboardType.GoToPaymentPageForPhones, ConstantStrings.PayInlineButtons, optionalUrls);
                        TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, ConstantStrings.OrReturnToMainMenu);
                        ResetFieldAndStep();
                    }
                    else if (newPaymentInputParams.Length == 1)//Showing 1 button
                    {
                        var WebserviceResult2 = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, newPaymentInputParams);

                        if (WebserviceResult2.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                        {
                            TelegramApi.ShowInlineKeyboard(UserInfo.UserId, KeyboardType.GoToPaymentPageSingle, ConstantStrings.PayInlineButton, WebserviceResult2.Parameters.PaymentLink);
                            TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, ConstantStrings.OrReturnToMainMenu);
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
        private void FixedLineBillInquiry(string messageText = "")
        {
            switch (UserInfo.FixedLineBillInquiryStep)
            {
                case FixedLineBillInquiryStep.None:
                    TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, ConstantStrings.FixedLineBillInquiryNone);
                    UserInfo.FixedLineBillInquiryStep = FixedLineBillInquiryStep.Inquiring;
                    break;

                case FixedLineBillInquiryStep.Inquiring:
                    var WebserviceResult = GhabzinoCoreApi.FixedLineHandler(UserInfo.Token, messageText);
                    if (WebserviceResult.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                    {
                        var formattedBill = FormatBillDetails(WebserviceResult) + System.Environment.NewLine;

                        var finalIsValid = WebserviceResult.Parameters.FinalTerm.ValidForPayment;
                        var midIsValid = WebserviceResult.Parameters.MidTerm.ValidForPayment;

                        if (finalIsValid && midIsValid)
                        {
                            DataHandler.SavePaymentInfo(UserInfo.UserId, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.FinalTerm.BillID, PaymentID = WebserviceResult.Parameters.FinalTerm.PaymentID }, new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.MidTerm.BillID, PaymentID = WebserviceResult.Parameters.MidTerm.PaymentID }, });
                            TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.AddPayment, formattedBill);
                            UserInfo.FixedLineBillInquiryStep = FixedLineBillInquiryStep.Inqueried;
                        }
                        else if (finalIsValid)
                        {
                            DataHandler.SavePaymentInfo(UserInfo.UserId, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.FinalTerm.BillID, PaymentID = WebserviceResult.Parameters.FinalTerm.PaymentID } });
                            TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.AddPayment, formattedBill);
                            UserInfo.FixedLineBillInquiryStep = FixedLineBillInquiryStep.Inqueried;
                        }
                        else if (midIsValid)
                        {
                            DataHandler.SavePaymentInfo(UserInfo.UserId, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.MidTerm.BillID, PaymentID = WebserviceResult.Parameters.MidTerm.PaymentID } });
                            TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.AddPayment, formattedBill);
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
                    var newPaymentInputParams = DataHandler.ReadPaymentInfo(UserInfo.UserId);

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
                        TelegramApi.ShowInlineKeyboard(UserInfo.UserId, KeyboardType.GoToPaymentPageForPhones, ConstantStrings.PayInlineButtons, optionalUrls);
                        TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, ConstantStrings.OrReturnToMainMenu);
                        ResetFieldAndStep();
                    }
                    else if (newPaymentInputParams.Length == 1)//Showing 1 button
                    {
                        var WebserviceResult2 = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, newPaymentInputParams);

                        if (WebserviceResult2.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                        {
                            TelegramApi.ShowInlineKeyboard(UserInfo.UserId, KeyboardType.GoToPaymentPageSingle, ConstantStrings.PayInlineButton, WebserviceResult2.Parameters.PaymentLink);
                            TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, ConstantStrings.OrReturnToMainMenu);
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
        private void TrafficFinesInquiry(string messageText = "")
        {
            switch (UserInfo.TrafficFinesInquiryStep)
            {
                case TrafficFinesInquiryStep.None:
                    TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, ConstantStrings.TrafficFinesInquiryNone);
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
                                    terraficFines[j] = new DataHandler.TerraficFines() { City = item.City, DateTime = item.DateTime, Delivery = item.Delivery, Location = item.Location, Type = item.Type, Amount = item.Amount, ValidForPayment = item.ValidForPayment };
                                    j++;
                                }
                            }
                            DataHandler.SavePaymentInfo(UserInfo.UserId, reportNewPaymentInputParams);//مشخصات پرداخت
                            DataHandler.SaveTerraficFinesInfo(UserInfo.UserId, terraficFines);//مشخصات خلافی
                            UserInfo.TrafficPage = 0;
                            DataHandler.SaveUserInfo(UserInfo);
                            TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ChooseOrAddAllPayment, formattedBill);
                            UserInfo.TrafficFinesInquiryStep = TrafficFinesInquiryStep.Inqueried;
                        }
                        else
                        {
                            ResetFieldAndStep();
                            ShowMainMenu(formattedBill);//???
                                                        //popup (هیچ جریمه ی قابل پرداختی وجود ندارد)
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
                    if (string.Equals(messageText, ProjectValues.payAllTraffic, StringComparison.OrdinalIgnoreCase)) //پرداخت قبض همه جرایم
                    {
                        var WebserviceResult2 = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, DataHandler.ReadPaymentInfo(UserInfo.UserId));
                        if (WebserviceResult2.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                        {
                            TelegramApi.ShowInlineKeyboard(UserInfo.UserId, KeyboardType.GoToPaymentPageSingle, ConstantStrings.PayInlineButton, WebserviceResult2.Parameters.PaymentLink);
                            TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, ConstantStrings.OrReturnToMainMenu);
                            ResetFieldAndStep();
                        }
                        else
                        {
                            //popup
                            ShowMainMenu(WebserviceResult2.Status.Description);//???
                        }
                    }
                    else //انتخاب جرایم برای پرداخت
                    {
                        string[] strParams = new string[3];
                        var allPagesPaymentInputParams = DataHandler.ReadPaymentInfo(UserInfo.UserId);
                        var allPagesCount = allPagesPaymentInputParams.Length;
                        var selectedPagesNumbersArray = DataHandler.ReadSelectedBillsInfo(UserInfo.UserId);
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
                        var allPagesDetails = DataHandler.ReadTerraficFinesInfo(UserInfo.UserId);
                        var currentPageDetail = allPagesDetails[UserInfo.TrafficPage];
                        //==========================================================================
                        if (messageText == ConstantStrings.Next)
                        {
                            if (UserInfo.TrafficPage >= allPagesCount - 1)
                            {
                                TelegramApi.AnswerCallback(UserInfo.UserId.ToString(), "همکنون در صفحه آخر هستید.");
                            }
                            else
                            {
                                UserInfo.TrafficPage = UserInfo.TrafficPage + 1;
                                currentPageDetail = allPagesDetails[UserInfo.TrafficPage];
                            }
                        }
                        else if (messageText == ConstantStrings.Previous)
                        {
                            if (UserInfo.TrafficPage <= 0)
                            {
                                TelegramApi.AnswerCallback(UserInfo.UserId.ToString(), "همکنون در صفحه اول هستید.");
                            }
                            else
                            {
                                UserInfo.TrafficPage = UserInfo.TrafficPage - 1;
                                currentPageDetail = allPagesDetails[UserInfo.TrafficPage];
                            }
                        }
                        else if (messageText == ConstantStrings.AddToPaymentList)
                        {
                            if (!currentPageDetail.ValidForPayment)
                            {
                                TelegramApi.AnswerCallback(UserInfo.UserId.ToString(), "این قبض برای پرداخت معتبر نیست.");
                            }
                            else if (selectedPagesNumbersArray.Contains(UserInfo.TrafficPage))
                            {
                                TelegramApi.AnswerCallback(UserInfo.UserId.ToString(), "همکنون در لیست پرداخت است.");
                            }
                            else
                            {
                                selectedPagesNumbersArray = selectedPagesNumbersArray.Concat(new int[] { UserInfo.TrafficPage }).ToArray();
                                //
                                for (int i = 0; i < allPagesPaymentInputParams.Length; i++)
                                {
                                    var pageInputParam = allPagesPaymentInputParams[i];

                                    if (pageInputParam.BillID == allPagesPaymentInputParams[UserInfo.TrafficPage].BillID && allPagesDetails[UserInfo.TrafficPage].ValidForPayment && i != UserInfo.TrafficPage)
                                    {
                                        selectedPagesNumbersArray = selectedPagesNumbersArray.Concat(new int[] { i }).ToArray();
                                    }
                                }
                                //
                                DataHandler.SaveSelectedBillsInfo(UserInfo.UserId, selectedPagesNumbersArray);

                                TelegramApi.AnswerCallback(UserInfo.UserId.ToString(), "به لیست اضافه شد.");
                            }
                        }
                        else if (messageText == ConstantStrings.RemoveFromPaymentList)
                        {
                            if (selectedPagesNumbersArray.Contains(UserInfo.TrafficPage))
                            {
                                selectedPagesNumbersArray = selectedPagesNumbersArray.RemoveAt(Array.IndexOf(selectedPagesNumbersArray, UserInfo.TrafficPage));
                                //
                                for (int i = 0; i < allPagesPaymentInputParams.Length; i++)
                                {
                                    if (allPagesPaymentInputParams[i].BillID == allPagesPaymentInputParams[UserInfo.TrafficPage].BillID)
                                    {
                                        if (selectedPagesNumbersArray.Contains(i))
                                        {
                                            selectedPagesNumbersArray = selectedPagesNumbersArray.RemoveAt(Array.IndexOf(selectedPagesNumbersArray, i));
                                        }
                                    }
                                }
                                //
                                DataHandler.SaveSelectedBillsInfo(UserInfo.UserId, selectedPagesNumbersArray);
                                TelegramApi.AnswerCallback(UserInfo.UserId.ToString(), "از لیست حذف شد.");
                            }
                            else
                            {
                                TelegramApi.AnswerCallback(UserInfo.UserId.ToString(), "همکنون در لیست پرداخت نیست.");
                            }
                        }
                        else if (messageText == ProjectValues.payOnline)
                        {
                            if (selectedPagesNumbersArray.Length == 0)
                            {
                                TelegramApi.AnswerCallback(UserInfo.UserId.ToString(), "حداقل یک قبض را انتخاب کنید.");
                            }
                            else if (webserviceResult2?.Status?.Code != ConstantStrings.WebserviceStatusSuccess || string.IsNullOrEmpty(webserviceResult2?.Parameters?.PaymentLink))
                            {
                                TelegramApi.AnswerCallback(UserInfo.UserId.ToString(), webserviceResult2.Status.Description);
                                //log
                            }
                            else
                            {
                                TelegramApi.AnswerCallback(UserInfo.UserId.ToString(), "در حال انتقال به درگاه بانک...");
                                //

                                ReportNewPaymentInputParams[] selectedPagesReportNewInputParams = new ReportNewPaymentInputParams[selectedPagesNumbersArray.Length];
                                for (int i = 0; i < selectedPagesNumbersArray.Length; i++)
                                {
                                    selectedPagesReportNewInputParams[i] = allPagesPaymentInputParams[selectedPagesNumbersArray[i]];
                                }

                                var WebserviceResult2 = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, selectedPagesReportNewInputParams);
                                if (WebserviceResult2.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                                {
                                    TelegramApi.ShowInlineKeyboard(UserInfo.UserId, KeyboardType.GoToPaymentPageSingle, ConstantStrings.PayInlineButton, WebserviceResult2.Parameters.PaymentLink);
                                    TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, ConstantStrings.OrReturnToMainMenu);
                                    ResetFieldAndStep();
                                }
                                else
                                {
                                    //popup
                                    ShowMainMenu(WebserviceResult2.Status.Description);//???
                                }


                                //
                                ResetFieldAndStep();
                            }
                        }
                        else if (messageText == ProjectValues.chooseToAdd)
                        {

                        }
                        else
                        {
                            //popup
                            Log.Test("last else in traffic");
                            ShowMainMenu(webserviceResult2.Status.Description);//???
                        }

                        var FormattedTerraficFines = FormatTerraficFinesDetails(UserInfo.TrafficPage, allPagesCount, currentPageDetail);

                        if (UserInfo.TrafficPage <= 0)
                        {
                            strParams[0] = "NoPrevious";
                        }
                        if (UserInfo.TrafficPage >= allPagesCount - 1)
                        {
                            strParams[0] = "NoNext";
                        }
                        if (!selectedPagesNumbersArray.Contains(UserInfo.TrafficPage))
                        {
                            strParams[1] = "AddButton";
                        }
                        strParams[2] = webserviceResult2?.Parameters?.PaymentLink;

                        TelegramApi.ShowInlineKeyboard(UserInfo.UserId, KeyboardType.ChooseToAddPayment, FormattedTerraficFines, strParams);
                        TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.AddPayment, "یا به صفحه اصلی باز گردید.");
                    }
                    break;

                default:
                    break;
            }
        }
        private void Bill(string messageText = "")
        {
            switch (UserInfo.BillStep)
            {
                case BillStep.None:
                    TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, "لطفا شناسه قبض را وارد نمایید:");
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
                        DataHandler.SavePaymentInfo(UserInfo.UserId, new ReportNewPaymentInputParams { BillID = messageText });
                        TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, "لطفا شناسه پرداخت را وارد نمایید:");
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
                        var webserviceResult = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, new ReportNewPaymentInputParams { BillID = DataHandler.ReadPaymentInfo(UserInfo.UserId)[0].BillID, PaymentID = messageText });
                        if (webserviceResult.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                        {
                            var formattedBill = FormatBillDetails(webserviceResult) + Environment.NewLine;//???

                            if (webserviceResult.Parameters.Bills[0].ValidForPayment)
                            {
                                DataHandler.SavePaymentInfo(UserInfo.UserId, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = webserviceResult.Parameters.Bills[0].BillID, PaymentID = webserviceResult.Parameters.Bills[0].PaymentID } });
                                TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.AddPayment, formattedBill);
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
                    var webserviceResult2 = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, DataHandler.ReadPaymentInfo(UserInfo.UserId));
                    if (webserviceResult2.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                    {
                        TelegramApi.ShowInlineKeyboard(UserInfo.UserId, KeyboardType.GoToPaymentPageSingle, ConstantStrings.PayInlineButton, webserviceResult2.Parameters.PaymentLink);
                        TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, ConstantStrings.OrReturnToMainMenu);
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
                    TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.History, "مایلید سوابق کدام پرداختی ها را ببینید:");
                    UserInfo.HistoryStep = HistoryStep.FieldDetermined;
                    break;

                case HistoryStep.FieldDetermined:
                    string strType = null;
                    if (messageText == ProjectValues.All)
                    {
                        strType = null;
                    }
                    else if (messageText == ProjectValues.WaterBillInquiry)
                    {
                        strType = "Water";
                    }
                    else if (messageText == ProjectValues.ElectricityBillInquiry)
                    {
                        strType = "Electric";
                    }
                    else if (messageText == ProjectValues.GasBillInquiry)
                    {
                        strType = "Gas";
                    }
                    else if (messageText == ProjectValues.MciMobileBillInquiry)
                    {
                        strType = "Mobile";
                    }
                    else if (messageText == ProjectValues.FixedLineBillInquiry)
                    {
                        strType = "Phone";
                    }
                    //else if (messageText == ProjectValues.Municipality)
                    //{
                    //    strType = "Municipality";
                    //}
                    else if (messageText == ProjectValues.TrafficFinesInquery)
                    {
                        strType = "Driving";
                    }
                    var webserviceResult = GhabzinoCoreApi.GetEndUserPaymentHistoryDetail(UserInfo.Token, strType);

                    ResetFieldAndStep();
                    TelegramApi.ShowKeyboard(UserInfo.UserId, KeyboardType.ReturnToMainMenu, FormatPaymentHistoryDetails(webserviceResult, messageText));
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
            UserInfo.BillStep = BillStep.None;
            UserInfo.HistoryStep = HistoryStep.None;

            DataHandler.SaveUserInfo(UserInfo);
        }
    }
}
