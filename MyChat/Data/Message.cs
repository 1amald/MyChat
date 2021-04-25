using System;
using System.ComponentModel.DataAnnotations;


namespace MyChat.Data
{
    public class Message
    {
        [Required]
        public int Id { get; set; }
        public AppUser Sender { get; set; }
        [Required]
        public string SenderName { get; set; }
        [Required]
        public string Text { get; set; }
        public string ShortDate { get; set; }
        [Required]
        public DateTime When { get; set; }

        public Message()
        {
            
        }
        public Message(AppUser u,string text)
        {
            SenderName = u.UserName;
            Text = text;
            When = DateTime.Now;
            ShortDate = When.ToShortTimeString();
        }
    }
}
