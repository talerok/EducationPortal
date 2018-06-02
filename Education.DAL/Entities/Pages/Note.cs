using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Education.DAL.Entities.Pages
{
    public class Note : Entity
    {
        public virtual User Owner { get; set; }
        public string Preview { get; set; }
        public DateTime Time { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public bool Published { get; set; }
    }
}