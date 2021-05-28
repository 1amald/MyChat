using System;
using System.ComponentModel.DataAnnotations;

namespace MyChat.Models
{
    public class Message
    {
        public Guid Id{ get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime When { get; set; }
        public string AvatarPath { get; set; }
        public string ShortDate { get; set; }
        public virtual AppUser Sender { get; set; }

        public Message()
        {

        }

        public Message(string userName,string text, string avatarPath)
        {
            UserName = userName;
            Text = text;
            AvatarPath = avatarPath;
            When = DateTime.Now;
            ShortDate = When.ToShortTimeString();
        }
        
    }
}
