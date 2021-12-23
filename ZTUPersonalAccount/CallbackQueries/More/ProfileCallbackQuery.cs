using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using ZTUPersonalAccount.Client;

namespace ZTUPersonalAccount.CallbackQueries
{
    public class ProfileCallbackQuery : ICallbackQuery
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly MoreCallbackQuery _moreCallbackQuery;

        public ProfileCallbackQuery(TelegramClient telegramClient, MoreCallbackQuery moreCallbackQuery)
        {
            _telegramBotClient = telegramClient.GetInstance();
            _moreCallbackQuery = moreCallbackQuery;
        }

        public async Task ExecuteAsync(CallbackQuery callbackQuery)
        {
            await _telegramBotClient.SendTextMessageAsync(chatId: callbackQuery.Message.Chat.Id,
                                                             text: "here your big profile");
            await _moreCallbackQuery.ExecuteAsync(callbackQuery);
        }
    }
}
