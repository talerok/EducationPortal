﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Education.DAL.Entities
{
    public class Group : Entity
    {
        public string Name { get; set; }
        public virtual ICollection<UserGroup> Users { get; set; }

        public Group()
        {
            Users = new List<UserGroup>();
        }
    }
}