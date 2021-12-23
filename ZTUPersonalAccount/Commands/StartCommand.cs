using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using ZTUPersonalAccount.Client;
using ZTUPersonalAccount.Models;
using ZTUPersonalAccount.Repositories;

namespace ZTUPersonalAccount.Commands
{
    public class StartCommand : ICommand
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ChatRepository _chatRep;

        public StartCommand(TelegramClient telegramClient, ChatRepository chatRepository)
        {
            _telegramBotClient = telegramClient.GetInstance();
            _chatRep = chatRepository;
        }

        public async Task ExecuteAsync(Message message)
        {
            Chat chat = message.Chat;
            ChatModel chatModel = await _chatRep.GetByChatId(chat.Id);
            if (chatModel == null)
                await _chatRep.Add(new ChatModel { ChatId = chat.Id, Username = chat.Username, FirstName = chat.FirstName, LastName = chat.LastName });

            await _telegramBotClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: "Розклад:",
                                                        replyMarkup: InlineKeyboards.KeyboardStart);
        }
    }
}
