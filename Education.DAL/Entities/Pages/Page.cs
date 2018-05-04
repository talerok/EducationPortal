using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Education.DAL.Entities.Pages
{
    public class Page : Entity
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public bool Published { get; set; }
        public virtual ICollection<Page> ChildPages { get; set; }
        public virtual Page ParentPage { get; set; }
    }
}