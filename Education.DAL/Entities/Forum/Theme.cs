using System;
using System.Collections.Generic;
using System.Text;

namespace Education.DAL.Entities
{
    public class Theme : Entity
    {
        public virtual ICollection<Message> Messages { get; set; }
        public bool Open { get; set; }
        public virtual Section Section { get; set; }
        public Theme(){
            Messages = new List<Message>();
        }
    }
}
