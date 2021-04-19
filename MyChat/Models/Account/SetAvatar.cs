using Microsoft.AspNetCore.Http;

namespace MyChat.Models.Account
{
    public class SetAvatar
    {
        public IFormFile Avatar { get; set; }
    }
}
