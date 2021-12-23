using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using ZTUPersonalAccount.Client;
using ZTUPersonalAccount.Repositories;

namespace ZTUPersonalAccount.Commands
{
    public class NotFoundCommand : ICommand
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ChatRepository _chatRep;

        public NotFoundCommand(TelegramClient telegramClient, ChatRepository chatRepository)
        {
            _telegramBotClient = telegramClient.GetInstance();
            _chatRep = chatRepository;
        }

        public async Task ExecuteAsync(Message message)
        {
            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, "Невідома команда");
        }
    }
}
