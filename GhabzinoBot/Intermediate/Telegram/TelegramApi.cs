using Ayantech.WebService;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace GhabzinoBot
{
    public class TelegramApi
    {
        private static KeyboardButton _btnReturnToMainMenu { get; set; }
        private static KeyboardButton _btnGoToPaymentPage { get; set; }
        private static KeyboardButton _btnGoToPaymentPageMidTerm { get; set; }
        private static KeyboardButton _btnGoToPaymentPageFullTerm { get; set; }
        private static KeyboardButton _btnGoToOnlinePayment { get; set; }
        private static KeyboardButton _btnGoToPaymentAllTraffic { get; set; }
        private static KeyboardButton _btnGoToChooseToAdd { get; set; }

        private static KeyboardButton _btnWaterBillInquiry { get; set; }
        private static KeyboardButton _btnGasBillInquiry { get; set; }
        private static KeyboardButton _btnElectricityBillInquiry { get; set; }

        private static KeyboardButton _btnTrafficFinesInquiry { get; set; }
        private static KeyboardButton _btnMciMobileBillInquiry { get; set; }
        private static KeyboardButton _btnFixedLineBillInquiry { get; set; }

        private static KeyboardButton _btnAll { get; set; }

        private static KeyboardButton _btnBills { get; set; }
        private static KeyboardButton _btnHistory { get; set; }
        private static KeyboardButton _btnRecipt { get; set; }
        //==================================================================================
        private static InlineKeyboardCallbackButton _btnInlineReturnToMainMenu { get; set; }
        private static InlineKeyboardUrlButton _btnInlineGoToPaymentPageSingle { get; set; }
        private static InlineKeyboardUrlButton _btnInlineGoToPaymentPageMultiple { get; set; }
        private static InlineKeyboardUrlButton _btnInlineGoToPaymentPageMidTerm { get; set; }
        private static InlineKeyboardUrlButton _btnInlineGoToPaymentPageFullTerm { get; set; }

        private static InlineKeyboardCallbackButton _btnInlineWaterBillInquiry { get; set; }
        private static InlineKeyboardCallbackButton _btnInlineGasBillInquiry { get; set; }
        private static InlineKeyboardCallbackButton _btnInlineElectricityBillInquiry { get; set; }

        private static InlineKeyboardCallbackButton _btnInlineTrafficFinesInquiry { get; set; }
        private static InlineKeyboardCallbackButton _btnInlineMciMobileBillInquiry { get; set; }
        private static InlineKeyboardCallbackButton _btnInlineFixedLineBillInquiry { get; set; }

        private static InlineKeyboardCallbackButton _btnInlineBills { get; set; }
        private static InlineKeyboardCallbackButton _btnInlineHistory { get; set; }
        private static InlineKeyboardCallbackButton _btnInlineRecipt { get; set; }

        private static InlineKeyboardCallbackButton _btnInlineNext { get; set; }
        private static InlineKeyboardCallbackButton _btnInlinePrevious { get; set; }
        private static InlineKeyboardCallbackButton _btnInlineAll { get; set; }
        private static InlineKeyboardCallbackButton _btnInlineAddToPaymentList { get; set; }
        private static InlineKeyboardCallbackButton _btninlineRemoveFromPaymentList { get; set; }


        public TelegramApi()
        {
            _btnReturnToMainMenu = new KeyboardButton(ProjectValues.returnToMainMenu);
            _btnGoToPaymentPage = new KeyboardButton(ProjectValues.payBill);
            _btnGoToPaymentPageMidTerm = new KeyboardButton(ProjectValues.payBillHalfTerm);
            _btnGoToPaymentPageFullTerm = new KeyboardButton(ProjectValues.payBillFullTerm);
            _btnGoToOnlinePayment = new KeyboardButton(ProjectValues.payOnline);
            _btnGoToPaymentAllTraffic = new KeyboardButton(ProjectValues.payAllTraffic);
            _btnGoToChooseToAdd = new KeyboardButton(ProjectValues.chooseToAdd);


            _btnWaterBillInquiry = new KeyboardButton(ProjectValues.WaterBillInquiry);
            _btnGasBillInquiry = new KeyboardButton(ProjectValues.GasBillInquiry);
            _btnElectricityBillInquiry = new KeyboardButton(ProjectValues.ElectricityBillInquiry);

            _btnTrafficFinesInquiry = new KeyboardButton(ProjectValues.TrafficFinesInquery);
            _btnMciMobileBillInquiry = new KeyboardButton(ProjectValues.MciMobileBillInquiry);
            _btnFixedLineBillInquiry = new KeyboardButton(ProjectValues.FixedLineBillInquiry);

            _btnAll = new KeyboardButton(ProjectValues.All);

            _btnBills = new KeyboardButton(ProjectValues.bill);
            _btnHistory = new KeyboardButton(ProjectValues.history);
            _btnRecipt = new KeyboardButton(ProjectValues.paymensts);
            //==================================================================================
            _btnInlineReturnToMainMenu = new InlineKeyboardCallbackButton(ProjectValues.returnToMainMenu, ProjectValues.returnToMainMenu);
            _btnInlineGoToPaymentPageSingle = new InlineKeyboardUrlButton(ProjectValues.payBill, "");
            _btnInlineGoToPaymentPageMultiple = new InlineKeyboardUrlButton(ProjectValues.payBills, "");
            _btnInlineGoToPaymentPageMidTerm = new InlineKeyboardUrlButton(ProjectValues.payBillHalfTerm, "");
            _btnInlineGoToPaymentPageFullTerm = new InlineKeyboardUrlButton(ProjectValues.payBillFullTerm, "");

            _btnInlineWaterBillInquiry = new InlineKeyboardCallbackButton(ProjectValues.WaterBillInquiry, ProjectValues.WaterBillInquiry);
            _btnInlineGasBillInquiry = new InlineKeyboardCallbackButton(ProjectValues.GasBillInquiry, ProjectValues.GasBillInquiry);
            _btnInlineElectricityBillInquiry = new InlineKeyboardCallbackButton(ProjectValues.ElectricityBillInquiry, ProjectValues.ElectricityBillInquiry);

            _btnInlineTrafficFinesInquiry = new InlineKeyboardCallbackButton(ProjectValues.TrafficFinesInquery, ProjectValues.TrafficFinesInquery);
            _btnInlineMciMobileBillInquiry = new InlineKeyboardCallbackButton(ProjectValues.MciMobileBillInquiry, ProjectValues.MciMobileBillInquiry);
            _btnInlineFixedLineBillInquiry = new InlineKeyboardCallbackButton(ProjectValues.FixedLineBillInquiry, ProjectValues.FixedLineBillInquiry);

            _btnInlineBills = new InlineKeyboardCallbackButton("پرداختی ها", "پرداختی ها");
            _btnInlineHistory = new InlineKeyboardCallbackButton("سوابق", "سوابق");
            _btnInlineRecipt = new InlineKeyboardCallbackButton("قبض", "قبض");

            _btnInlineNext = new InlineKeyboardCallbackButton(ConstantStrings.Next, ConstantStrings.Next);
            _btnInlinePrevious = new InlineKeyboardCallbackButton(ConstantStrings.Previous, ConstantStrings.Previous);
            _btnInlineAll = new InlineKeyboardCallbackButton(ProjectValues.payAllTraffic, ProjectValues.payAllTraffic);
            _btnInlineAddToPaymentList = new InlineKeyboardCallbackButton(ConstantStrings.AddToPaymentList, ConstantStrings.AddToPaymentList);
            _btninlineRemoveFromPaymentList = new InlineKeyboardCallbackButton(ConstantStrings.RemoveFromPaymentList, ConstantStrings.RemoveFromPaymentList);
        }

        private static KeyboardButton[][] CreateKeyboard(KeyboardType keyboardType)
        {
            KeyboardButton[][] keyboard = null;

            switch (keyboardType)
            {
                case KeyboardType.None:
                    //ERROR
                    break;
                case KeyboardType.MainMenu:
                    keyboard = new KeyboardButton[][] { new KeyboardButton[] { _btnWaterBillInquiry, _btnGasBillInquiry, _btnElectricityBillInquiry }, new KeyboardButton[] { _btnTrafficFinesInquiry, _btnMciMobileBillInquiry, _btnFixedLineBillInquiry, }, new KeyboardButton[] { _btnBills, _btnHistory, /*_btnRecipt,*/ }, };
                    break;
                case KeyboardType.ReturnToMainMenu:
                    keyboard = new KeyboardButton[][] { new KeyboardButton[] { _btnReturnToMainMenu, }, };
                    break;
                case KeyboardType.GoToPaymentPageSingle:
                    keyboard = new KeyboardButton[][] { new KeyboardButton[] { _btnGoToPaymentPage }, new KeyboardButton[] { _btnReturnToMainMenu } };
                    break;
                case KeyboardType.GoToPaymentPageForPhones:
                    keyboard = new KeyboardButton[][] { new KeyboardButton[] { _btnGoToPaymentPage }, new KeyboardButton[] { _btnReturnToMainMenu } };
                    break;
                case KeyboardType.AddPayment:
                    keyboard = new KeyboardButton[][] { new KeyboardButton[] { _btnGoToOnlinePayment, }, new KeyboardButton[] { _btnInlineReturnToMainMenu, }, };
                    break;
                case KeyboardType.ChooseOrAddAllPayment:
                    keyboard = new KeyboardButton[][] { new KeyboardButton[] { _btnGoToPaymentAllTraffic }, new KeyboardButton[] { _btnGoToChooseToAdd }, new KeyboardButton[] { _btnReturnToMainMenu } };
                    break;
                case KeyboardType.History:
                    keyboard = new KeyboardButton[][] { new KeyboardButton[] { _btnAll }, new KeyboardButton[] { _btnWaterBillInquiry, _btnGasBillInquiry, _btnElectricityBillInquiry }, new KeyboardButton[] { _btnTrafficFinesInquiry, _btnMciMobileBillInquiry, _btnFixedLineBillInquiry }, };
                    break;

                default:
                    //ERROR
                    break;
            }

            return keyboard;
        }
        private static InlineKeyboardButton[][] CreateInlineKeyboard(KeyboardType keyboardType, params string[] optionalUrls)
        {
            InlineKeyboardButton[][] keyboard = null;

            switch (keyboardType)
            {
                case KeyboardType.None:
                    //ERROR
                    break;
                case KeyboardType.MainMenu:
                    keyboard = new InlineKeyboardButton[][] { new InlineKeyboardButton[] { _btnInlineWaterBillInquiry, _btnInlineGasBillInquiry, _btnInlineElectricityBillInquiry, }, new InlineKeyboardButton[] { _btnInlineTrafficFinesInquiry, _btnInlineMciMobileBillInquiry, _btnInlineFixedLineBillInquiry, }, new InlineKeyboardButton[] { _btnInlineBills, _btnInlineHistory, _btnInlineRecipt, }, };
                    break;
                case KeyboardType.ReturnToMainMenu:
                    keyboard = new InlineKeyboardButton[][] { new InlineKeyboardButton[] { _btnInlineReturnToMainMenu, }, };
                    break;
                case KeyboardType.GoToPaymentPageSingle:
                    _btnInlineGoToPaymentPageSingle.Url = optionalUrls?[0];
                    keyboard = new InlineKeyboardButton[][] { new InlineKeyboardButton[] { _btnInlineGoToPaymentPageSingle, }, };
                    break;
                case KeyboardType.GoToPaymentPageForPhones:
                    _btnInlineGoToPaymentPageFullTerm.Url = optionalUrls?[0];
                    _btnInlineGoToPaymentPageMidTerm.Url = optionalUrls?[1];
                    keyboard = new InlineKeyboardButton[][] { new InlineKeyboardButton[] { _btnInlineGoToPaymentPageFullTerm, }, new InlineKeyboardButton[] { _btnInlineGoToPaymentPageMidTerm, }, };
                    break;
                case KeyboardType.ChooseToAddPayment:
                    _btnInlineGoToPaymentPageSingle.Url = "https://msdn.microsoft.com/en-us/library/bb675163(v=vs.90).aspx";

                    //if (optionalUrls != null)
                    //{
                    //    _btnInlineGoToPaymentPageSingle.Url = optionalUrls?[2];
                    //}
                    InlineKeyboardButton[] navigationButtons = null;
                    if (string.Equals(optionalUrls?[0], "NoPrevious", System.StringComparison.OrdinalIgnoreCase))
                    {
                        navigationButtons = new InlineKeyboardButton[] { _btnInlineNext, };
                    }
                    else if (string.Equals(optionalUrls?[0], "NoNext", System.StringComparison.OrdinalIgnoreCase))
                    {
                        navigationButtons = new InlineKeyboardButton[] { _btnInlinePrevious, };
                    }
                    else
                    {
                        navigationButtons = new InlineKeyboardButton[] { _btnInlinePrevious, _btnInlineNext, };
                    }

                    keyboard = new InlineKeyboardButton[][] { navigationButtons, new InlineKeyboardButton[] { string.Equals(optionalUrls?[1], "AddButton", System.StringComparison.OrdinalIgnoreCase) ? _btnInlineAddToPaymentList : _btninlineRemoveFromPaymentList, }, new InlineKeyboardButton[] { _btnInlineAll, }, /*new InlineKeyboardButton[] { _btnInlineGoToPaymentPageSingle, },*/ };
                    break;

                default:
                    //ERROR
                    break;
            }

            return keyboard;
        }
        public static void ShowKeyboard(ChatId chatId, KeyboardType keyboardType, string messageToSend = "")
        {
            //ProjectValues.Bot.SendChatActionAsync(chatId, Telegram.Bot.Types.Enums.ChatAction.Typing);//Not for slow connections
            var keyboard = CreateKeyboard(keyboardType);

            var markup = new ReplyKeyboardMarkup(keyboard, resizeKeyboard: true);
            ProjectValues.Bot.SendTextMessageAsync(chatId, messageToSend, replyMarkup: markup);
        }
        public static void ShowInlineKeyboard(ChatId chatId, KeyboardType keyboardType, string messageToSend = "", params string[] optionalUrls)
        {
            //ProjectValues.Bot.SendChatActionAsync(chatId, Telegram.Bot.Types.Enums.ChatAction.Typing);//Not for slow connections
            var inlineKeyboard = CreateInlineKeyboard(keyboardType, optionalUrls);
            var inlineMarkup = new InlineKeyboardMarkup(inlineKeyboard);
            ProjectValues.Bot.SendTextMessageAsync(chatId, messageToSend, replyMarkup: inlineMarkup);
        }
        //public static void ShowInlineKeyboardEdit(ChatId chatId, KeyboardType keyboardType, string messageToSend = "", params string[] optionalUrls)
        //{
        //    //ProjectValues.Bot.SendChatActionAsync(chatId, Telegram.Bot.Types.Enums.ChatAction.Typing);//Not for slow connections
        //    var inlineKeyboard = CreateInlineKeyboard(keyboardType, optionalUrls);
        //    var inlineMarkup = new InlineKeyboardMarkup(inlineKeyboard);
        //    ProjectValues.Bot.EditMessageTextAsync(chatId, messageToSend, replyMarkup: inlineMarkup);
        //}

        public static void AnswerCallback(string callbackQueryId, string text = null, bool showAlert = false, string url = null)
        {
            ProjectValues.Bot.AnswerCallbackQueryAsync(callbackQueryId, text, showAlert, url);
        }
        public static void AnswerInline(string callbackQueryId)
        {
            //ProjectValues.Bot.AnswerInlineQueryAsync(callbackQueryId,Telegram.Bot.Types.InlineQueryResults.InlineQueryResultType.);
        }

    }
}
