using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using ZTUPersonalAccount.Client;
using ZTUPersonalAccount.Commands;

namespace ZTUPersonalAccount.CallbackQueries
{
    public class NotFoundCallbackQuery : ICallbackQuery
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly StartCommand _startCommand;

        public NotFoundCallbackQuery(TelegramClient telegramClient, StartCommand startCommand)
        {
            _telegramBotClient = telegramClient.GetInstance();
            _startCommand = startCommand;
        }

        public async Task ExecuteAsync(CallbackQuery callbackQuery)
        {
            await _telegramBotClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Помилка CallbackQuery");
        }
    }
}
