using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using ZTUPersonalAccount.Client;
using ZTUPersonalAccount.States;
using ZTUPersonalAccount.States.LoginState;

namespace ZTUPersonalAccount.Commands
{
    public class LoginCommand : ICommand
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly StateFactory _stateFactory;

        public LoginCommand(TelegramClient telegramClient, StateFactory stateFactory)
        {
            _telegramBotClient = telegramClient.GetInstance();
            _stateFactory = stateFactory;
        }

        public async Task ExecuteAsync(Message message)
        {
            _stateFactory.RemoveStateAndData(message.Chat.Id);

            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, "Авторизація в особистому кабінеті:");

            string state = WriteUserNameState.Name;
            _stateFactory.SetState(message.Chat.Id, state);
            await _stateFactory.CreateState(state).ExecuteAsync(message);
        }
    }
}
