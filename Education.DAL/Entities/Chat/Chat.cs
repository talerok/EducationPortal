using System;
using System.Collections.Generic;
using System.Text;

namespace Education.DAL.Entities.Chat
{
    class Chat
    {
        public User Owner { get; set; }
        public ICollection<UserChat> UserChat { get; set; }
        public ICollection<ChatMessage> ChatMessage { get; set; }
    }
}
