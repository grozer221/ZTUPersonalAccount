using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using ZTUPersonalAccount.Client;

namespace ZTUPersonalAccount.States
{
    class NotFoundState : IState
    {
        public const string Name = "NotFoundState";
        public string GetName()
        {
            return Name;
        }

        private readonly ITelegramBotClient _telegramBotClient;


        public NotFoundState(TelegramClient telegramClient)
        {
            _telegramBotClient = telegramClient.GetInstance();
        }

        public async Task ExecuteAsync(Message message)
        {
            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, "Стейт не знадений");
        }
    }
}
