using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyChat.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string AvatarPath { get; set; } = "/Avatars/1.jpg";
        [Required]
        public string Status { get; set; } = "I`m new here I do not know that the status can be changed in the settings";
        public virtual List<Message> Messages { get; set; }
    }
}
