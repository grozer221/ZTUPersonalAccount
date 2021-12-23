using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using ZTUPersonalAccount.Client;
using ZTUPersonalAccount.Models;
using ZTUPersonalAccount.Repositories;
using ZTUPersonalAccount.States;

namespace ZTUPersonalAccount.Commands
{
    public class StartCommand : ICommand
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ChatRepository _chatRep;
        private readonly StateFactory _stateFactory;

        public StartCommand(TelegramClient telegramClient, ChatRepository chatRepository, StateFactory stateFactory)
        {
            _telegramBotClient = telegramClient.GetInstance();
            _chatRep = chatRepository;
            _stateFactory = stateFactory;
        }

        public async Task ExecuteAsync(Message message)
        {
            _stateFactory.RemoveStateAndData(message.Chat.Id);

            Chat chat = message.Chat;
            ChatModel chatModel = await _chatRep.GetByChatIdAsync(chat.Id);
            if (chatModel == null)
                await _chatRep.AddAsync(new ChatModel { ChatId = chat.Id, Username = chat.Username, FirstName = chat.FirstName, LastName = chat.LastName });

            await _telegramBotClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                          text: "Розклад:",
                                                          replyMarkup: InlineKeyboards.KeyboardStart);
        }
    }
}
