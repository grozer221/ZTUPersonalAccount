using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ZTUPersonalAccount.Commands
{
    public interface ICommand
    {
        public Task ExecuteAsync(Message message);
    }
}
