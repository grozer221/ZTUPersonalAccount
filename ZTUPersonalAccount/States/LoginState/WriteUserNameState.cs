using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using ZTUPersonalAccount.Client;

namespace ZTUPersonalAccount.States.LoginState
{
    class WriteUserNameState : IState
    {
        public const string Name = "WriteUserNameState";
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly StateFactory _stateFactory;

        public WriteUserNameState(TelegramClient telegramClient, StateFactory stateFactory)
        {
            _telegramBotClient = telegramClient.GetInstance();
            _stateFactory = stateFactory;
        }

        public string GetName()
        {
            return Name;
        }

        public async Task ExecuteAsync(Message message)
        {
            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"Введіть логін:");
        }
    }
}
