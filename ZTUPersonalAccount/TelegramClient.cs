using System;
using Telegram.Bot;

namespace ZTUPersonalAccount.Client
{
    public class TelegramClient
    {
        private static ITelegramBotClient _telegramBotClient;

        private static readonly object _lockObject = new object();

        private readonly string _telegramBotKey;

        public TelegramClient()
        {
            _telegramBotKey = Environment.GetEnvironmentVariable("BOT_TOKEN");
        }

        public ITelegramBotClient GetInstance()
        {
            if (_telegramBotClient == null)
            {
                lock (_lockObject)
                {
                    if (_telegramBotClient == null)
                    {
                        _telegramBotClient = new TelegramBotClient(_telegramBotKey);
                    }
                }
            }
            return _telegramBotClient;
        }
    }
}
