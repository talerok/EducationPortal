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
        public bool Edited { get; set; }
    }
}
