using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using ZTUPersonalAccount.Client;

namespace ZTUPersonalAccount.States.LoginState
{
    public class WritePasswordState : IState
    {
        public const string Name = "WritePasswordState";
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly StateFactory _stateFactory;

        public WritePasswordState(TelegramClient telegramClient, StateFactory stateFactory)
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
            _stateFactory.SetData(message.Chat.Id, WriteUserNameState.Name, message.Text);
            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"Введіть пароль:");
        }


    }
}
