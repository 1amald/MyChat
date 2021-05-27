using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using MyChat.Core;
using MyChat.Models;
using System;
using System.Threading.Tasks;

namespace MyChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMessageRepository messageRepository;
        public async Task Send(string messageText)
        {
            AppUser u = await _userManager.GetUserAsync(Context.User);
            Message m = new Message(u.UserName, messageText, u.AvatarPath);
            await Clients.All.SendAsync("Send", m);
            messageRepository.AddMessage(m);
        }
        public ChatHub(UserManager<AppUser> userManager, IMessageRepository messageRepository)
        {
            _userManager = userManager;
            this.messageRepository = messageRepository;
        }
    }
}
