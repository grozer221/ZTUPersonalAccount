using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ZTUPersonalAccount.CallbackQueries
{
    public interface ICallbackQuery
    {
        public Task ExecuteAsync(CallbackQuery callbackQuery);
    }
}
