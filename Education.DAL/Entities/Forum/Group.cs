using System;
using System.Collections.Generic;
using System.Text;

namespace Education.DAL.Entities
{
    public class Group : Entity
    {
        public string Name { get; set; }
        public string Logo { get; set; }
        public bool Open { get; set; }
        public virtual ICollection<UserGroup> Users { get; set; }
        public virtual ICollection<Section> Sections { get; set; }
        public Group()
        {
            Users = new List<UserGroup>();
            Sections = new List<Section>();
        }
    }
}
