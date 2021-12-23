using Microsoft.Extensions.DependencyInjection;
using System;
using Telegram.Bot.Types;
using ZTUPersonalAccount.CallbackQueries;

namespace ZTUPersonalAccount.CommandFactory
{
    public class TelegramCallbackQueryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public TelegramCallbackQueryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICallbackQuery CreateCallbackQuery(CallbackQuery callbackQuery)
        {
            return callbackQuery.Data switch
            {
                InlineKeyboards.ButtonStartToday => _serviceProvider.GetRequiredService<TodayCallbackQuery>(),
                InlineKeyboards.ButtonStartTommorow => _serviceProvider.GetRequiredService<TommorowCallbackQuery>(),
                InlineKeyboards.ButtonStartTwoWeeks => _serviceProvider.GetRequiredService<TwoWeeksCallbackQuery>(),
                InlineKeyboards.ButtonStartMore => _serviceProvider.GetRequiredService<MoreCallbackQuery>(),
                InlineKeyboards.ButtonStartMoreProfile => _serviceProvider.GetRequiredService<ProfileCallbackQuery>(),
                InlineKeyboards.ButtonStartMoreBack => _serviceProvider.GetRequiredService<BackCallbackQuery>(),
                _ => _serviceProvider.GetRequiredService<NotFoundCallbackQuery>()
            };
        }
    }
}
