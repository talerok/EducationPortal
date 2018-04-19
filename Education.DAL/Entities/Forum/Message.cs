using System;
using System.Collections.Generic;
using System.Text;

namespace Education.DAL.Entities
{
    public class Message : Entity
    {
        public virtual User Owner { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public virtual User LastEditor { get; set; }
        public virtual DateTime LastEditTime { get; set; }
        public virtual Theme Theme { get; set; }
    }
}
