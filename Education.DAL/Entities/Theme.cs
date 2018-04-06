using System;
using System.Collections.Generic;
using System.Text;

namespace Education.DAL.Entities
{
    public class Theme : Entity
    {
        public Message Description { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
