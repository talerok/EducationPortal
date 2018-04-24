using Education.BLL.Logic.Interfaces;
using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Logic.Rules
{
    public class SectionRules : ISectionRules
    {
        private IGroupRules GroupRules;

        public SectionRules(IGroupRules groupRules)
        {
            GroupRules = groupRules;
        }

        public bool CanCreate(User user, Group group)
        {
            return GroupRules.CanEdit(user, group);
        }

        public bool CanDelete(User user, Section section)
        {
            return CanEdit(user, section);
        }

        public bool CanEdit(User user, Section section)
        {
            if (section == null) throw new ArgumentNullException("section");
            if (section.Group == null) throw new NullReferenceException("section.Group must be not null");
            return GroupRules.CanEdit(user, section.Group);
        }

        public bool CanRead(User user, Section section)
        {
            if (section == null) throw new ArgumentNullException("section");
            if (section.Group == null) throw new NullReferenceException("section.Group must be not null");
            return GroupRules.CanRead(user, section.Group);
        }
    }
}
