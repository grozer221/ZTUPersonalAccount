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
using ZTUPersonalAccount.Commands;
using ZTUPersonalAccount.Repositories;
using ZTUPersonalAccount.States;
using ZTUPersonalAccount.States.LoginState;

namespace ZTUPersonalAccount
{
    class Program
    {
        public static IServiceProvider Services { get; set; }

        public static TelegramClient TelegramClient { get; set; }
        public static ITelegramBotClient TelegramBotClient { get; set; }
        public static CommandFactory CommandFactory { get; set; }
        public static CallbackQueryFactory CallbackQueryFactory { get; set; }
        public static StateFactory StateFactory { get; set; }

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

            CommandFactory = Services.GetRequiredService<CommandFactory>();
            CallbackQueryFactory = Services.GetRequiredService<CallbackQueryFactory>();
            StateFactory = Services.GetRequiredService<StateFactory>();

            User me = await TelegramBotClient.GetMeAsync();
            Console.WriteLine($"Start listening for @{me.Username}");
            Thread.Sleep(int.MaxValue);
        }

        private static void RegisterDependencies()
        {
            Services = new ServiceCollection()
                .AddSingleton<TelegramClient>()

                // Commands
                .AddSingleton<CommandFactory>()

                .AddSingleton<LoginCommand>()
                .AddSingleton<NotFoundCommand>()
                .AddSingleton<StartCommand>()

                // CallbackQueries
                .AddSingleton<CallbackQueryFactory>()

                .AddSingleton<ProfileCallbackQuery>()
                .AddSingleton<BackCallbackQuery>()

                .AddSingleton<MoreCallbackQuery>()
                .AddSingleton<NotFoundCallbackQuery>()
                .AddSingleton<TodayCallbackQuery>()
                .AddSingleton<TommorowCallbackQuery>()
                .AddSingleton<TwoWeeksCallbackQuery>()

                // States
                .AddSingleton<StateFactory>()

                .AddSingleton<NotFoundState>()

                .AddSingleton<LoginSubmitState>()
                .AddSingleton<WriteUserNameState>()
                .AddSingleton<WritePasswordState>()

                // DB
                .AddDbContext<AppDBContext>(options => options.UseMySQL(AppDBContext.GetConnectionString()))
                .AddSingleton<ChatRepository>()
                .AddSingleton<PersonalAccountRepository>()
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

            string state = StateFactory.GetState(message.Chat.Id);
            if (string.IsNullOrEmpty(state))
                await CommandFactory.CreateCommand(message).ExecuteAsync(message);
            else
            {
                IState nextState = StateFactory.Next(message.Chat.Id);
                if(nextState != null)
                {
                    await StateFactory.CreateState(nextState.GetName()).ExecuteAsync(message);
                }
                else
                {
                    await CommandFactory.CreateCommand(message).ExecuteAsync(message);
                }
            }
        }

        private static async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.EditMessageReplyMarkupAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
            await CallbackQueryFactory.CreateCallbackQuery(callbackQuery).ExecuteAsync(callbackQuery);

                //await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Помилка CallbackQuery");
        }

        private static Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            Console.WriteLine($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }
    }
}
