using Ayantech.WebService;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace GhabzinoBot
{
    public static class TelegramComponent
    {
        private static readonly KeyboardButton btnReturnToMainMenu = new KeyboardButton(ConstantStrings.returnToMainMenu);
        private static readonly KeyboardButton btnGoToPaymentPage = new KeyboardButton(ConstantStrings.payBill);
        private static readonly KeyboardButton btnGoToPaymentPageMidTerm = new KeyboardButton(ConstantStrings.payBillHalfTerm);
        private static readonly KeyboardButton btnGoToPaymentPageFullTerm = new KeyboardButton(ConstantStrings.payBillFullTerm);
        private static readonly KeyboardButton btnGoToOnlinePayment = new KeyboardButton(ConstantStrings.payOnline);

        private static readonly KeyboardButton btnWaterBillInquiry = new KeyboardButton(ConstantStrings.WaterBillInquiry);
        private static readonly KeyboardButton btnGasBillInquiry = new KeyboardButton(ConstantStrings.GasBillInquiry);
        private static readonly KeyboardButton btnElectricityBillInquiry = new KeyboardButton(ConstantStrings.ElectricityBillInquiry);

        private static readonly KeyboardButton btnTrafficFinesInquiry = new KeyboardButton(ConstantStrings.TrafficFinesInquery);
        private static readonly KeyboardButton btnMciMobileBillInquiry = new KeyboardButton(ConstantStrings.MciMobileBillInquiry);
        private static readonly KeyboardButton btnFixedLineBillInquiry = new KeyboardButton(ConstantStrings.FixedLineBillInquiry);

        private static readonly KeyboardButton btnBills = new KeyboardButton("پرداختی ها");
        private static readonly KeyboardButton btnHistory = new KeyboardButton("سوابق");
        private static readonly KeyboardButton btnRecipt = new KeyboardButton("قبض");
        //==================================================================================
        private static readonly InlineKeyboardCallbackButton btnInlineReturnToMainMenu = new InlineKeyboardCallbackButton(ConstantStrings.returnToMainMenu, ConstantStrings.returnToMainMenu);
        private static InlineKeyboardUrlButton btnInlineGoToPaymentPage = new InlineKeyboardUrlButton(ConstantStrings.payBill, "");
        private static InlineKeyboardUrlButton btnInlineGoToPaymentPageMidTerm = new InlineKeyboardUrlButton(ConstantStrings.payBillHalfTerm, "");
        private static InlineKeyboardUrlButton btnInlineGoToPaymentPageFullTerm = new InlineKeyboardUrlButton(ConstantStrings.payBillFullTerm, "");

        private static readonly InlineKeyboardCallbackButton btnInlineWaterBillInquiry = new InlineKeyboardCallbackButton(ConstantStrings.WaterBillInquiry, ConstantStrings.WaterBillInquiry);
        private static readonly InlineKeyboardCallbackButton btnInlineGasBillInquiry = new InlineKeyboardCallbackButton(ConstantStrings.GasBillInquiry, ConstantStrings.GasBillInquiry);
        private static readonly InlineKeyboardCallbackButton btnInlineElectricityBillInquiry = new InlineKeyboardCallbackButton(ConstantStrings.ElectricityBillInquiry, ConstantStrings.ElectricityBillInquiry);

        private static readonly InlineKeyboardCallbackButton btnInlineTrafficFinesInquiry = new InlineKeyboardCallbackButton(ConstantStrings.TrafficFinesInquery, ConstantStrings.TrafficFinesInquery);
        private static readonly InlineKeyboardCallbackButton btnInlineMciMobileBillInquiry = new InlineKeyboardCallbackButton(ConstantStrings.MciMobileBillInquiry, ConstantStrings.MciMobileBillInquiry);
        private static readonly InlineKeyboardCallbackButton btnInlineFixedLineBillInquiry = new InlineKeyboardCallbackButton(ConstantStrings.FixedLineBillInquiry, ConstantStrings.FixedLineBillInquiry);

        private static readonly InlineKeyboardCallbackButton btnInlineBills = new InlineKeyboardCallbackButton("پرداختی ها", "پرداختی ها");
        private static readonly InlineKeyboardCallbackButton btnInlineHistory = new InlineKeyboardCallbackButton("سوابق", "سوابق");
        private static readonly InlineKeyboardCallbackButton btnInlineRecipt = new InlineKeyboardCallbackButton("قبض", "قبض");

        private static KeyboardButton[][] CreateKeyboard(KeyboardType keyboardType)
        {
            KeyboardButton[][] keyboard = null;

            switch (keyboardType)
            {
                case KeyboardType.None:
                    //ERROR
                    break;
                case KeyboardType.MainMenu:
                    keyboard = new KeyboardButton[][] { new KeyboardButton[] { btnWaterBillInquiry, btnGasBillInquiry, btnElectricityBillInquiry }, new KeyboardButton[] { btnTrafficFinesInquiry, btnMciMobileBillInquiry, btnFixedLineBillInquiry }, new KeyboardButton[] { btnBills, btnHistory, btnRecipt }, };
                    break;
                case KeyboardType.ReturnToMainMenu:
                    keyboard = new KeyboardButton[][] { new KeyboardButton[] { btnReturnToMainMenu, }, };
                    break;
                case KeyboardType.GoToPaymentPageSingle:
                    keyboard = new KeyboardButton[][] { new KeyboardButton[] { btnGoToPaymentPage }, new KeyboardButton[] { btnReturnToMainMenu } };
                    break;
                case KeyboardType.GoToPaymentPageForPhones:
                    keyboard = new KeyboardButton[][] { new KeyboardButton[] { btnGoToPaymentPage }, new KeyboardButton[] { btnReturnToMainMenu } };
                    break;
                case KeyboardType.AddPayment:
                    keyboard = new KeyboardButton[][] { new KeyboardButton[] { btnGoToOnlinePayment, }, new KeyboardButton[] { btnInlineReturnToMainMenu, }, };
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
                    keyboard = new InlineKeyboardButton[][] { new InlineKeyboardButton[] { btnInlineWaterBillInquiry, btnInlineGasBillInquiry, btnInlineElectricityBillInquiry, }, new InlineKeyboardButton[] { btnInlineTrafficFinesInquiry, btnInlineMciMobileBillInquiry, btnInlineFixedLineBillInquiry, }, new InlineKeyboardButton[] { btnInlineBills, btnInlineHistory, btnInlineRecipt, }, };
                    break;
                case KeyboardType.ReturnToMainMenu:
                    keyboard = new InlineKeyboardButton[][] { new InlineKeyboardButton[] { btnInlineReturnToMainMenu, }, };
                    break;
                case KeyboardType.GoToPaymentPageSingle:
                    btnInlineGoToPaymentPage.Url = optionalUrls?[0];
                    keyboard = new InlineKeyboardButton[][] { new InlineKeyboardButton[] { btnInlineGoToPaymentPage, }, };
                    break;
                case KeyboardType.GoToPaymentPageForPhones:
                    btnInlineGoToPaymentPageFullTerm.Url = optionalUrls?[0];
                    btnInlineGoToPaymentPageMidTerm.Url = optionalUrls?[1];
                    keyboard = new InlineKeyboardButton[][] { new InlineKeyboardButton[] { btnInlineGoToPaymentPageFullTerm, }, new InlineKeyboardButton[] { btnInlineGoToPaymentPageMidTerm, }, };
                    break;

                default:
                    //ERROR
                    break;
            }

            return keyboard;
        }
        public static void ShowKeyboard(ChatId chatId, KeyboardType keyboardType, string messageToSend = "")
        {
            ProjectValues.Bot.SendChatActionAsync(chatId, Telegram.Bot.Types.Enums.ChatAction.Typing);
            var keyboard = CreateKeyboard(keyboardType);

            var markup = new ReplyKeyboardMarkup(keyboard, resizeKeyboard: true);
            ProjectValues.Bot.SendTextMessageAsync(chatId, messageToSend, replyMarkup: markup);
        }
        public static void ShowInlineKeyboard(ChatId chatId, KeyboardType keyboardType, string messageToSend = "", params string[] optionalUrls)
        {
            ProjectValues.Bot.SendChatActionAsync(chatId, Telegram.Bot.Types.Enums.ChatAction.Typing);
            var inlineKeyboard = CreateInlineKeyboard(keyboardType, optionalUrls);
            var inlineMarkup = new InlineKeyboardMarkup(inlineKeyboard);
            ProjectValues.Bot.SendTextMessageAsync(chatId, messageToSend, replyMarkup: inlineMarkup);
        }
    }
}
