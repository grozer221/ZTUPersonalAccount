using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ZTUPersonalAccount.Client;
using ZTUPersonalAccount.Commands;
using ZTUPersonalAccount.Models;
using ZTUPersonalAccount.Repositories;
using ZTUPersonalAccount.ViewModels;

namespace ZTUPersonalAccount.CallbackQueries
{
    public class TwoWeeksCallbackQuery : ICallbackQuery
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly StartCommand _startCommand;
        private readonly ChatRepository _chatRep;

        public TwoWeeksCallbackQuery(TelegramClient telegramClient, StartCommand startCommand, ChatRepository chatRep)
        {
            _telegramBotClient = telegramClient.GetInstance();
            _startCommand = startCommand;
            _chatRep = chatRep;
        }

        public async Task ExecuteAsync(CallbackQuery callbackQuery)
        {
            Chat chat = callbackQuery.Message.Chat;
            Console.WriteLine($"#{chat.Id} {chat.FirstName} {chat.LastName} @{chat.Username}: schedule 2 weeks");
            ChatModel chatModel = await _chatRep.GetByChatIdAsync(callbackQuery.From.Id);
            var schedule = await Requests.GetScheduleForTwoWeeksAsync(chatModel.GroupName, chatModel.SubGroupNumber);
            foreach (KeyValuePair<string, Dictionary<string, List<Subject>>> week in schedule)
            {
                await _telegramBotClient.SendTextMessageAsync(chat.Id, $"🆘🆘🆘🆘🆘🆘🆘  <strong>{week.Key}</strong>  🆘🆘🆘🆘🆘🆘🆘", parseMode: ParseMode.Html);
                foreach (KeyValuePair<string, List<Subject>> day in week.Value)
                {
                    var extraText = schedule[week.Key][day.Key].Count > 3 ? "🤯🧨" : "";
                    string text = $"📅 <i><strong>{day.Key}</strong></i>  {extraText}\n";
                    for (int i = 0; i < day.Value.Count; i++)
                    {
                        text += $"<strong>{i + 1}) {day.Value[i].Name}</strong> / {day.Value[i].Type} {day.Value[i].Cabinet} / ⏱️{day.Value[i].Time} / 👨‍🏫 {day.Value[i].Teacher}\n";
                    }
                    await _telegramBotClient.SendTextMessageAsync(chat.Id, text, parseMode: ParseMode.Html);
                }

            }
            await _startCommand.ExecuteAsync(callbackQuery.Message);
        }
    }
}
