using Microsoft.Extensions.DependencyInjection;
using System;
using Telegram.Bot.Types;

namespace ZTUPersonalAccount.CallbackQueries
{
    public class CallbackQueryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CallbackQueryFactory(IServiceProvider serviceProvider)
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
