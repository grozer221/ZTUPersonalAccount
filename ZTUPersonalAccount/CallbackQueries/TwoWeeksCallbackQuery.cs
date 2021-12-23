using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using ZTUPersonalAccount.Client;
using ZTUPersonalAccount.Commands;

namespace ZTUPersonalAccount.CallbackQueries
{
    public class TwoWeeksCallbackQuery : ICallbackQuery
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly StartCommand _startCommand;

        public TwoWeeksCallbackQuery(TelegramClient telegramClient, StartCommand startCommand)
        {
            _telegramBotClient = telegramClient.GetInstance();
            _startCommand = startCommand;
        }

        public async Task ExecuteAsync(CallbackQuery callbackQuery)
        {
            await _telegramBotClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "here shedule for 2 weeks");
            await _startCommand.ExecuteAsync(callbackQuery.Message);
        }
    }
}
