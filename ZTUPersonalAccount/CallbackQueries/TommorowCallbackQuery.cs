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
    public class TommorowCallbackQuery : ICallbackQuery
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly StartCommand _startCommand;
        private readonly ChatRepository _chatRep;

        public TommorowCallbackQuery(TelegramClient telegramClient, StartCommand startCommand, ChatRepository chatRep)
        {
            _telegramBotClient = telegramClient.GetInstance();
            _startCommand = startCommand;
            _chatRep = chatRep;
        }

        public async Task ExecuteAsync(CallbackQuery callbackQuery)
        {
            Chat chat = callbackQuery.Message.Chat;
            Console.WriteLine($"#{chat.Id} {chat.FirstName} {chat.LastName} @{chat.Username}: schedule for tommorow");
            ChatModel chatModel = await _chatRep.GetByChatIdAsync(callbackQuery.From.Id);
            List<Subject> subjects = await Requests.GetScheduleForTomorrowAsync(chatModel.GroupName, chatModel.SubGroupNumber);
            foreach(var subject in subjects)
            {
                await _telegramBotClient.SendTextMessageAsync(chat.Id, $"<strong>{subject.Name}</strong> / 🚪 {subject.Type} {subject.Cabinet} / ⏱️{subject.Time} / 👨‍🏫 {subject.Teacher}", parseMode: ParseMode.Html);
            }
            await _startCommand.ExecuteAsync(callbackQuery.Message);
        }
    }
}
