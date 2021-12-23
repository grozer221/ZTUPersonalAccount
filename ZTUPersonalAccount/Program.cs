using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ZTUPersonalAccount.CallbackQueries;
using ZTUPersonalAccount.Client;
using ZTUPersonalAccount.CommandFactory;
using ZTUPersonalAccount.Commands;
using ZTUPersonalAccount.Repositories;

namespace ZTUPersonalAccount
{
    class Program
    {
        public static IServiceProvider Services { get; set; }

        public static TelegramClient TelegramClient { get; set; }
        public static ITelegramBotClient TelegramBotClient { get; set; }
        public static TelegramCommandFactory TelegramCommandFactory { get; set; }
        public static TelegramCallbackQueryFactory TelegramCallbackQueryFactory { get; set; }

        public static async Task Main()
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            RegisterDependencies();

            TelegramClient = Services.GetRequiredService<TelegramClient>();
            TelegramBotClient = TelegramClient.GetInstance();
            TelegramBotClient.StartReceiving(HandleUpdateAsync,
                                             HandleErrorAsync,
                                             new() { AllowedUpdates = { } });

            TelegramCommandFactory = Services.GetRequiredService<TelegramCommandFactory>();
            TelegramCallbackQueryFactory = Services.GetRequiredService<TelegramCallbackQueryFactory>();

            User me = await TelegramBotClient.GetMeAsync();
            Console.WriteLine($"Start listening for @{me.Username}");
            Thread.Sleep(int.MaxValue);
        }

        private static void RegisterDependencies()
        {
            Services = new ServiceCollection()
                .AddSingleton<TelegramClient>()


                .AddSingleton<TelegramCommandFactory>()

                .AddSingleton<StartCommand>()
                

                .AddSingleton<TelegramCallbackQueryFactory>()

                .AddSingleton<ProfileCallbackQuery>()
                .AddSingleton<BackCallbackQuery>()

                .AddSingleton<MoreCallbackQuery>()
                .AddSingleton<NotFoundCallbackQuery>()
                .AddSingleton<TodayCallbackQuery>()
                .AddSingleton<TommorowCallbackQuery>()
                .AddSingleton<TwoWeeksCallbackQuery>()


                .AddDbContext<AppDBContext>(options => options.UseMySQL(AppDBContext.GetConnectionString()))
                .AddSingleton<ChatRepository>()
                //.AddHttpClient()
                .BuildServiceProvider();
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(botClient, update.Message!),
                UpdateType.EditedMessage => BotOnMessageReceived(botClient, update.EditedMessage!),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(botClient, update.CallbackQuery!),
                _ => UnknownUpdateHandlerAsync(botClient, update)
            };

            try { await handler; }
            catch (Exception exception) { await HandleErrorAsync(botClient, exception, cancellationToken); }
        }

        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private static async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            if (message.Type != MessageType.Text)
                return;

            await TelegramCommandFactory.CreateCommand(message).ExecuteAsync(message);
        }

        private static async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.EditMessageReplyMarkupAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
            await TelegramCallbackQueryFactory.CreateCallbackQuery(callbackQuery).ExecuteAsync(callbackQuery);

                //await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Помилка CallbackQuery");
        }

        private static Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            Console.WriteLine($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }
    }
}
