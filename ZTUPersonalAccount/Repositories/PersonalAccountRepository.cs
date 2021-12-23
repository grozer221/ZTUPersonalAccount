using ZTUPersonalAccount.Models;
using System.Threading.Tasks;
using System.Linq;

namespace ZTUPersonalAccount.Repositories
{
    public class PersonalAccountRepository
    {
        private readonly AppDBContext _ctx;
        private readonly ChatRepository _chatRep;
        public PersonalAccountRepository(AppDBContext ctx, ChatRepository chatRep)
        {
            _ctx = ctx;
            _chatRep = chatRep;
        }
        
        public async Task<PersonalAccountModel> AddByChatIdAsync(long chatId, PersonalAccountModel personalAccount)
        {
            ChatModel chat = await _chatRep.GetByChatIdIncludedPersonalAccountAsync(chatId);
            chat.PersonalAccount = personalAccount;
            await _ctx.SaveChangesAsync();
            return personalAccount;
        }

        public async Task RemoveByChatId(long chatId)
        {
            ChatModel chat = await _chatRep.GetByChatIdIncludedPersonalAccountAsync(chatId);
            _ctx.PersonalAccounts.Remove(chat.PersonalAccount);
            await _ctx.SaveChangesAsync();
        }
    }
}
