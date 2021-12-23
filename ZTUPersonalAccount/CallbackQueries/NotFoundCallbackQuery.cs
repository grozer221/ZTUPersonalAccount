using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using ZTUPersonalAccount.Client;

namespace ZTUPersonalAccount.CallbackQueries
{
    public class NotFoundCallbackQuery : ICallbackQuery
    {
        private readonly ITelegramBotClient _telegramBotClient;

        public NotFoundCallbackQuery(TelegramClient telegramClient)
        {
            _telegramBotClient = telegramClient.GetInstance();
        }

        public async Task ExecuteAsync(CallbackQuery callbackQuery)
        {
            await _telegramBotClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Помилка CallbackQuery");
        }
    }
}
