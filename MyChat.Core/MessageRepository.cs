using MyChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyChat.Core
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext db;
        //private readonly DbSet<Message> messages;

        public MessageRepository(AppDbContext context)
        {
            db = context;
        }

        public void AddMessage(Message message)
        {
            db.Messages.Add(message);
            db.SaveChanges();
        }

        public List<Message> GetMessages(int skipCount,int takeCount)
        {
            var messages = db.Messages.OrderByDescending(m => m.When).Skip(skipCount).Take(takeCount).ToList();
            return messages;
        }
    }
}
