using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using MyChat.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace MyChat.Models
{
    public class ChatHub: Hub
    {
        UserManager<AppUser> um;
        public async Task Send(string userName,string messageText)
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
