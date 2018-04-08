using System;
using System.Collections.Generic;
using System.Text;

namespace Education.DAL.Entities
{
    public class Section : Entity
    {
        public string Name { get; set; }
        public virtual ICollection<Theme> Themes { get; set; }

        public Section()
        {
            Themes = new List<Theme>();
        }
    }
}
