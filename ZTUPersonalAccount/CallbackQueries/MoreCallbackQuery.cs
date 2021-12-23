using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using ZTUPersonalAccount.Client;

namespace ZTUPersonalAccount.CallbackQueries
{
    public class MoreCallbackQuery : ICallbackQuery
    {
        private readonly ITelegramBotClient _telegramBotClient;

        public MoreCallbackQuery(TelegramClient telegramClient)
        {
            _telegramBotClient = telegramClient.GetInstance();
        }

        public async Task ExecuteAsync(CallbackQuery callbackQuery)
        {
            await _telegramBotClient.SendTextMessageAsync(chatId: callbackQuery.Message.Chat.Id,
                                                 text: "Більше:",
                                                 replyMarkup: InlineKeyboards.KeyboardStartMore);
        }
    }
}
