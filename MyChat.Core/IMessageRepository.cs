using MyChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChat.Core
{
    public interface IMessageRepository
    {
        public List<Message> GetMessages(int skipCount,int takeCount);
        public void AddMessage(Message message);
    }
}
