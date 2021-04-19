using Microsoft.AspNetCore.Identity;
namespace MyChat.Data
{
    public class User : IdentityUser
    {
        public string AvatarPath { get; set; } = "mychat.jpg";
        public string Status { get; set; } = "Я тут недавно и еще не знаю, что статус можно поменять в настройках";
    }
}
