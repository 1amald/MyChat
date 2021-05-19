using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using MyChat.Core;
using MyChat.Models;
using System.Threading.Tasks;

namespace MyChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<AppUser> um;
        private readonly IMessageRepository messageRepository;
        public async Task Send(Message message)
        {
            AppUser u = await um.FindByNameAsync(message.UserName);
            message.AvatarPath = u.AvatarPath;
            message.Sender = u;
            await Clients.All.SendAsync("Send", message);
            messageRepository.AddMessage(message);
        }
        public ChatHub(UserManager<AppUser> userManager, IMessageRepository messageRepository)
        {
            um = userManager;
            this.messageRepository = messageRepository;
        }
    }
}
