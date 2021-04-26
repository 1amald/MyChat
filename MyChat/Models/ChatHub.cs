using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using MyChat.Data;
using System.Threading.Tasks;

namespace MyChat.Models
{
    public class ChatHub: Hub
    {
        private readonly UserManager<AppUser> um;
        AppDbContext db;
        public async Task Send(Message message)
        {
            AppUser u = await um.FindByNameAsync(message.UserName);
            message.AvatarPath = u.AvatarPath;
            message.ShortDate = message.When.ToShortTimeString();
            await Clients.All.SendAsync("Send", message);
            db.Messages.Add(message);
            await db.SaveChangesAsync();
        }
        public ChatHub(UserManager<AppUser> userManager,AppDbContext db)
        {
            um = userManager;
            this.db = db;
        }
    }
}
