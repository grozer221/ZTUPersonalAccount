using Telegram.Bot.Types.ReplyMarkups;

namespace ZTUPersonalAccount
{
    public static class InlineKeyboards
    {
        public const string ButtonStartToday = "ButtonStartToday";
        public const string ButtonStartTommorow = "ButtonStartTommorow";
        public const string ButtonStartTwoWeeks = "ButtonStartTwoWeeks";
        public const string ButtonStartMore = "ButtonStartMore";

        public static InlineKeyboardMarkup KeyboardStart = new(
            new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Сьогодні", ButtonStartToday),
                    InlineKeyboardButton.WithCallbackData("Завтра", ButtonStartTommorow),
                    InlineKeyboardButton.WithCallbackData("2 тижні", ButtonStartTwoWeeks),
                    InlineKeyboardButton.WithCallbackData("Більше", ButtonStartMore),
                },
            });
        
        
        public const string ButtonStartMoreProfile = "ButtonStartMoreProfile";
        public const string ButtonStartMoreBack = "ButtonStartMoreBack";

        public static InlineKeyboardMarkup KeyboardStartMore = new(
            new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Профіль", ButtonStartMoreProfile),
                    InlineKeyboardButton.WithCallbackData("Назад", ButtonStartMoreBack),
                },
            });
    }
}
