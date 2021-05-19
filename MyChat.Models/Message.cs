using System;
using System.ComponentModel.DataAnnotations;

namespace MyChat.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime When { get; set; }
        public string AvatarPath { get; set; }
        public virtual AppUser Sender { get; set; }

        public Message()
        {

        }
    }
}
