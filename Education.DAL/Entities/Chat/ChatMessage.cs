using System;
using System.Collections.Generic;
using System.Text;

namespace Education.DAL.Entities.Chat
{
    class ChatMessage
    {
        public User Owner { get; set; }
        public string Text { get; set; }
    }
}
