using System;
using System.Collections.Generic;
using System.Text;

namespace Education.DAL.Entities
{
    public class Theme : Entity
    {
        public virtual Message Description { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

        public Theme(){
            Messages = new List<Message>();
        }
    }
}
