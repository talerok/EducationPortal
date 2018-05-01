using Education.BLL.Logic.Interfaces;
using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Education.BLL.Logic.Rules
{
    public class GroupRules : IGroupRules
    {
        public bool CanCreate(User user)
        {
            if (user == null) return false;
            if (user.Level > 1) return true;
            else return false;
        }

        public bool CanEdit(User user, Group group)
        {
            if (group == null) throw new ArgumentNullException("group");
            if (user == null) return false;
            if (user.Level > 1) return true;

            var usergroup = group.Users.FirstOrDefault(x => x.User == user);
            if (usergroup != null && usergroup.Status == UserGroupStatus.owner)
                return true;

            return false;
        }

        public bool CanDelete(User user, Group group)
        {
            return CanCreate(user);
        }

        public bool CanRead(User user, Group group)
        {
            if (group == null) throw new ArgumentNullException("group");
            if (group.Open) return true;
            else
            {
                if (user == null) return false;
                if (user.Level > 0) return true;
                var usergroup = group.Users.FirstOrDefault(x => x.User == user);
                if (usergroup == null || usergroup.Status == UserGroupStatus.request)
                    return false;
            }
            return true;
        }

        public bool CanControlUsers(User user, Group group)
        {
            return CanEdit(user, group);
        }
    }
}
