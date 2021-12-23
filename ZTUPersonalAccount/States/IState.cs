using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ZTUPersonalAccount.States
{
    public interface IState
    {
        string GetName();
        Task ExecuteAsync(Message message);
    }
}
