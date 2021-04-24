using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyChat.Data
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string AvatarPath { get; set; } = "/Avatars/1.jpg";
        [Required]
        public string Status { get; set; } = "Я тут недавно и еще не знаю, что статус можно поменять в настройках";
        public virtual List<Message> Messages { get; set; }
        public DateTime LastAction { get; set; }
        public AppUser()
        {
            //Messages = new HashSet<Message>();
        }
    }
}
