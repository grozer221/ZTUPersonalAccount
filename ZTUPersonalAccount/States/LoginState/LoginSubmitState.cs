using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using ZTUPersonalAccount.Client;
using ZTUPersonalAccount.Commands;
using ZTUPersonalAccount.Models;
using ZTUPersonalAccount.Repositories;

namespace ZTUPersonalAccount.States.LoginState
{
    class LoginSubmitState : IState
    {
        public const string Name = "LoginSubmitState";
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly StartCommand _startCommand;
        private readonly StateFactory _stateFactory;
        private readonly PersonalAccountRepository _personalAccountRep;

        public LoginSubmitState(TelegramClient telegramClient, StartCommand startCommand, StateFactory stateFactory, PersonalAccountRepository personalAccountRep)
        {
            _telegramBotClient = telegramClient.GetInstance();
            _startCommand = startCommand;
            _stateFactory = stateFactory;
            _personalAccountRep = personalAccountRep;
        }

        public string GetName()
        {
            return Name;
        }

        public async Task ExecuteAsync(Message message)
        {
            string userName = _stateFactory.GetData(message.Chat.Id)[WriteUserNameState.Name];
            string password = message.Text;
            Console.WriteLine($"UserName: {userName},  Password: {password}");
            IEnumerable<string> cookie = await Requests.LoginInPersonalAccount(userName, password);
            if (cookie != null)
            {
                PersonalAccountModel personalAccount = new PersonalAccountModel
                {
                    Username = userName,
                    Password = password,
                    Cookie = JsonConvert.SerializeObject(cookie),
                };
                await _personalAccountRep.AddByChatIdAsync(message.Chat.Id, personalAccount);
                await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"Ви успішно авторизувалися");
            }
            else
            {
                await _personalAccountRep.RemoveByChatId(message.Chat.Id);
                await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, $"Не правильний логін або пароль");
            }
            await _startCommand.ExecuteAsync(message);
        }
    }
}
