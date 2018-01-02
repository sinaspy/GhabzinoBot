using Ayantech.WebService;
using GhabzinoBot.GhabzinoService;
using System.Diagnostics;
using Telegram.Bot.Types;

namespace GhabzinoBot
{
    public class UpdateHandler
    {
        public UserInfo UserInfo { get; set; }

        public void Bot_OnUpdate(Update update)
        {
            var sw = Stopwatch.StartNew();

            if (!(update.Type == Telegram.Bot.Types.Enums.UpdateType.MessageUpdate || update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQueryUpdate))
            {
                Log.Warn($"(Update ID: {update.Id})(Wrong Update Type: {update.Type.ToString()})", sw.Elapsed.TotalMilliseconds);//???
                return;
            }

            int userId = update?.Message?.From?.Id == null ? 0 : update.Message.From.Id;
            userId = update?.CallbackQuery?.From?.Id == null ? userId : update.CallbackQuery.From.Id;
            UserInfo = DataHandler.ReadUserInfo(userId);

            var messageText = update?.Message?.Text != null ? update.Message.Text : update.CallbackQuery.Data;

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
                    TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.MainMenu, ConstantStrings.MainMenuDescription);
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
                    UserInfo.TrafficFinesBillInquiryStep = TrafficFinesInquiryStep.None;
                    TrafficFinesInquiry();
                    break;

                case ConstantStrings.paymensts:
                    //
                    break;
                case ConstantStrings.history:
                    //
                    break;
                case ConstantStrings.bill:
                    //
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
                    break;
                case UserField.History:
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
                    break;
                case UserField.Bill:
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
        private void Register(string userInputText = "")
        {
            switch (UserInfo.UserState)
            {
                case UserState.NotRegistered:
                    TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, "برای ثبت نام و استفاده از قبضینو، لطفا شماره موبایل خود را وارد کنید:");

                    UserInfo.UserState = UserState.RequestingActivationCode;
                    DataHandler.SaveUserInfo(UserInfo);
                    break;

                case UserState.RequestingActivationCode:
                    Mobile mobile = new Mobile();
                    mobile.Number = Helper.ToEnglishNumber(userInputText);
                    mobile.IsNumberContentValid();

                    if (!mobile.NumberContentIsValid)
                    {
                        TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, "شماره موبایل وارد شده صحیح نیست، لطفا شماره موبایل خود را وارد کنید:");

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
                            TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, "لطفا کد پیامکی را که چند لحظه دیگر به دستتان می رسد، وارد کنید:");

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

                                TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.MainMenu, $"ثبت نام با موفقیت انجام شد.{System.Environment.NewLine}{ConstantStrings.MainMenuDescription}");
                            }
                            else//Login Unsuccessful
                            {
                                UserInfo.UserState = UserState.NotRegistered;
                                DataHandler.SaveUserInfo(UserInfo);

                                TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, webserviceOutput2.Status.Description);
                            }
                        }
                    }
                    else//RequestActivationCode Unsuccessful
                    {
                        UserInfo.UserState = UserState.NotRegistered;
                        DataHandler.SaveUserInfo(UserInfo);

                        TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, webserviceOutput.Status.Description);
                    }
                    break;

                case UserState.ActivationCodeSent:
                    var webserviceOutput3 = GhabzinoCoreApi.Login(UserInfo.Mobile, userInputText);

                    if (webserviceOutput3.Status.Code == ConstantStrings.WebserviceStatusSuccess)//Login Successful
                    {
                        UserInfo.Token = webserviceOutput3.Parameters.Token;
                        UserInfo.UserState = UserState.Registered;
                        DataHandler.SaveUserInfo(UserInfo);

                        TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.MainMenu, $"ثبت نام با موفقیت انجام شد.{System.Environment.NewLine}{ConstantStrings.MainMenuDescription}");
                    }
                    else//Login Unsuccessful
                    {
                        UserInfo.UserState = UserState.NotRegistered;
                        DataHandler.SaveUserInfo(UserInfo);

                        TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, webserviceOutput3.Status.Description);
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
            TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.MainMenu, message);
        }
        private void WaterBillInquiry(string messageText = "")
        {
            try
            {
                switch (UserInfo.WaterBillInquiryStep)
                {
                    case WaterBillInquiryStep.None:
                        TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, ConstantStrings.WaterBillInquiryNone);
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
                                TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
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
                            TelegramComponent.ShowInlineKeyboard(UserInfo.UserID, KeyboardType.GoToPaymentPageSingle, ConstantStrings.PayInlineButton, WebserviceResult2.Parameters.PaymentLink);
                            TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, "یا به صفحه اصلی باز گردید.");
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
                        TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, ConstantStrings.GasBillInquiryNone);
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
                                TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
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
                            TelegramComponent.ShowInlineKeyboard(UserInfo.UserID, KeyboardType.GoToPaymentPageSingle, ConstantStrings.PayInlineButton, WebserviceResult2.Parameters.PaymentLink);
                            TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, "یا به صفحه اصلی باز گردید.");
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
                        TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, ConstantStrings.ElectricityBillInquiryNone);
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
                                TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
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
                            TelegramComponent.ShowInlineKeyboard(UserInfo.UserID, KeyboardType.GoToPaymentPageSingle, ConstantStrings.PayInlineButton, WebserviceResult2.Parameters.PaymentLink);
                            TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, "یا به صفحه اصلی باز گردید.");
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
                        TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, ConstantStrings.MciMobileBillInquiryNone);
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
                                TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
                                UserInfo.FixedLineBillInquiryStep = FixedLineBillInquiryStep.Inqueried;
                            }
                            else if (finalIsValid)
                            {
                                DataHandler.SavePaymentInfo(UserInfo.Token, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.FinalTerm.BillID, PaymentID = WebserviceResult.Parameters.FinalTerm.PaymentID } });
                                TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
                                UserInfo.FixedLineBillInquiryStep = FixedLineBillInquiryStep.Inqueried;
                            }
                            else if (midIsValid)
                            {
                                DataHandler.SavePaymentInfo(UserInfo.Token, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.MidTerm.BillID, PaymentID = WebserviceResult.Parameters.MidTerm.PaymentID } });
                                TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
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
                            TelegramComponent.ShowInlineKeyboard(UserInfo.UserID, KeyboardType.GoToPaymentPageForPhones, ConstantStrings.PayInlineButtons, optionalUrls);
                            TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, "یا به صفحه اصلی باز گردید.");
                            ResetFieldAndStep();
                        }
                        else if (newPaymentInputParams.Length == 1)//Showing 1 button
                        {
                            var WebserviceResult2 = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, newPaymentInputParams);

                            if (WebserviceResult2.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                            {
                                TelegramComponent.ShowInlineKeyboard(UserInfo.UserID, KeyboardType.GoToPaymentPageSingle, ConstantStrings.PayInlineButton, WebserviceResult2.Parameters.PaymentLink);
                                TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, "یا به صفحه اصلی باز گردید.");
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
                        TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, ConstantStrings.FixedLineBillInquiryNone);
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
                                TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
                                UserInfo.FixedLineBillInquiryStep = FixedLineBillInquiryStep.Inqueried;
                            }
                            else if (finalIsValid)
                            {
                                DataHandler.SavePaymentInfo(UserInfo.Token, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.FinalTerm.BillID, PaymentID = WebserviceResult.Parameters.FinalTerm.PaymentID } });
                                TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
                                UserInfo.FixedLineBillInquiryStep = FixedLineBillInquiryStep.Inqueried;
                            }
                            else if (midIsValid)
                            {
                                DataHandler.SavePaymentInfo(UserInfo.Token, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.MidTerm.BillID, PaymentID = WebserviceResult.Parameters.MidTerm.PaymentID } });
                                TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
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
                            TelegramComponent.ShowInlineKeyboard(UserInfo.UserID, KeyboardType.GoToPaymentPageForPhones, ConstantStrings.PayInlineButtons, optionalUrls);
                            TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, "یا به صفحه اصلی باز گردید.");
                            ResetFieldAndStep();
                        }
                        else if (newPaymentInputParams.Length == 1)//Showing 1 button
                        {
                            var WebserviceResult2 = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, newPaymentInputParams);

                            if (WebserviceResult2.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                            {
                                TelegramComponent.ShowInlineKeyboard(UserInfo.UserID, KeyboardType.GoToPaymentPageSingle, ConstantStrings.PayInlineButton, WebserviceResult2.Parameters.PaymentLink);
                                TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, "یا به صفحه اصلی باز گردید.");
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
                switch (UserInfo.FixedLineBillInquiryStep)
                {
                    case FixedLineBillInquiryStep.None:
                        TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, ConstantStrings.TrafficFinesInquiryNone);
                        UserInfo.TrafficFinesBillInquiryStep = TrafficFinesInquiryStep.Inquiring;
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
                                TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
                                UserInfo.FixedLineBillInquiryStep = FixedLineBillInquiryStep.Inqueried;
                            }
                            else if (finalIsValid)
                            {
                                DataHandler.SavePaymentInfo(UserInfo.Token, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.FinalTerm.BillID, PaymentID = WebserviceResult.Parameters.FinalTerm.PaymentID } });
                                TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
                                UserInfo.FixedLineBillInquiryStep = FixedLineBillInquiryStep.Inqueried;
                            }
                            else if (midIsValid)
                            {
                                DataHandler.SavePaymentInfo(UserInfo.Token, new ReportNewPaymentInputParams[] { new ReportNewPaymentInputParams { BillID = WebserviceResult.Parameters.MidTerm.BillID, PaymentID = WebserviceResult.Parameters.MidTerm.PaymentID } });
                                TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.AddPayment, formattedBill);
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
                            TelegramComponent.ShowInlineKeyboard(UserInfo.UserID, KeyboardType.GoToPaymentPageForPhones, ConstantStrings.PayInlineButtons, optionalUrls);
                            TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, "یا به صفحه اصلی باز گردید.");
                            ResetFieldAndStep();
                        }
                        else if (newPaymentInputParams.Length == 1)//Showing 1 button
                        {
                            var WebserviceResult2 = GhabzinoCoreApi.ReportNewPayment(UserInfo.Token, newPaymentInputParams);

                            if (WebserviceResult2.Status.Code == ConstantStrings.WebserviceStatusSuccess)
                            {
                                TelegramComponent.ShowInlineKeyboard(UserInfo.UserID, KeyboardType.GoToPaymentPageSingle, ConstantStrings.PayInlineButton, WebserviceResult2.Parameters.PaymentLink);
                                TelegramComponent.ShowKeyboard(UserInfo.UserID, KeyboardType.ReturnToMainMenu, "یا به صفحه اصلی باز گردید.");
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

        private void ResetFieldAndStep()
        {
            UserInfo.UserField = UserField.None;
            UserInfo.WaterBillInquiryStep = WaterBillInquiryStep.None;
            UserInfo.GasBillInquiryStep = GasBillInquiryStep.None;
            UserInfo.ElectricityBillInquiryStep = ElectricityBillInquiryStep.None;
            UserInfo.MciMobileBillInquiryStep = MciMobileBillInquiryStep.None;
            UserInfo.FixedLineBillInquiryStep = FixedLineBillInquiryStep.None;
            UserInfo.TrafficFinesBillInquiryStep = TrafficFinesInquiryStep.None;
            DataHandler.SaveUserInfo(UserInfo);
        }
    }
}
