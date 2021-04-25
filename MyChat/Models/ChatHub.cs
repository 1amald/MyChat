using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using MyChat.Data;
using System.Threading.Tasks;

namespace MyChat.Models
{
    public class ChatHub: Hub
    {
        UserManager<AppUser> um;
        public async Task Send(object message)
        {
            AppUser user = await um.FindByNameAsync(userName);
            string avatarPath = user.AvatarPath;
            await Clients.All.SendAsync("Send", userName,messageText,avatarPath);
        }
        public ChatHub(UserManager<AppUser> userManager)
        {
            um = userManager;
        }
    }
}
