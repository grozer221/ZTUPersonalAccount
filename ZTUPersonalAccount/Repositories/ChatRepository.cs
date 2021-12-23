using Microsoft.EntityFrameworkCore;
using ZTUPersonalAccount.Models;
using System.Threading.Tasks;

namespace ZTUPersonalAccount.Repositories
{
    public class ChatRepository
    {
        private readonly AppDBContext _ctx;
        public ChatRepository(AppDBContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<ChatModel> GetByChatIdAsync(long chatId)
        {
            return await _ctx.Chats.FirstOrDefaultAsync(c => c.ChatId == chatId);
        }
        
        public async Task<ChatModel> GetByChatIdIncludedPersonalAccountAsync(long chatId)
        {
            return await _ctx.Chats.Include(c => c.PersonalAccount).FirstOrDefaultAsync(c => c.ChatId == chatId);
        }
        
        public async Task<ChatModel> AddAsync(ChatModel chatModel)
        {
            await _ctx.Chats.AddAsync(chatModel);
            await _ctx.SaveChangesAsync();
            return chatModel;
        }
    }
}
