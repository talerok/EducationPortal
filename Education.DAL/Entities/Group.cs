using System;
using System.Collections.Generic;
using System.Text;

namespace Education.DAL.Entities
{
    public class Group : Entity
    {
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<User> Owners { get; set; }
    }
}
