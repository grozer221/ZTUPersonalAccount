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
        public async Task<ChatModel> GetByChatId(long chatId)
        {
            return await _ctx.Chats.FirstOrDefaultAsync(c => c.ChatId == chatId);
        }
        
        public async Task<ChatModel> Add(ChatModel chatModel)
        {
            await _ctx.Chats.AddAsync(chatModel);
            await _ctx.SaveChangesAsync();
            return chatModel;
        }
    }
}
