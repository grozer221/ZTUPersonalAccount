using Microsoft.Extensions.DependencyInjection;
using System;
using Telegram.Bot.Types;
using ZTUPersonalAccount.Commands;

namespace ZTUPersonalAccount.CommandFactory
{
    public class TelegramCommandFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public TelegramCommandFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICommand CreateCommand(Message command)
        {
            return command.Text switch
            {
                "/start" => _serviceProvider.GetRequiredService<StartCommand>(),
                _ => _serviceProvider.GetRequiredService<StartCommand>()
            };
        }
    }
}
