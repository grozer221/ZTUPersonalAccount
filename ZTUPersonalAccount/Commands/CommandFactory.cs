using Microsoft.Extensions.DependencyInjection;
using System;
using Telegram.Bot.Types;

namespace ZTUPersonalAccount.Commands
{
    public class CommandFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICommand CreateCommand(Message message)
        {
            return message.Text switch
            {
                "/start" => _serviceProvider.GetRequiredService<StartCommand>(),
                "/login" => _serviceProvider.GetRequiredService<LoginCommand>(),
                _ => _serviceProvider.GetRequiredService<StartCommand>()
            };
        }
    }
}
