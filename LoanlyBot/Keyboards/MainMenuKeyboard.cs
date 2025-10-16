using LoanlyBot.Commands;
using Telegram.Bot.Types.ReplyMarkups;

namespace LoanlyBot.Keyboards
{
    public static class MainMenuKeyboard
    {
        public static ReplyKeyboardMarkup Get()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] {CommandConstants.ADD_LOAN, CommandConstants.ADD_PAYMENT},
                new KeyboardButton[] {CommandConstants.CHECK_LOANS, CommandConstants.PAYMENT_HISTORY }
            })
            {
                ResizeKeyboard = true
            };
        }
    }
}
