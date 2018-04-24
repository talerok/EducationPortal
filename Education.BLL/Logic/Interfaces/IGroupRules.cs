using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Logic.Interfaces
{
    public interface IGroupRules
    {
        bool CanCreate(User user);

        bool CanEdit(User user, Group group);

        bool CanDelete(User user, Group group);

        bool CanRead(User user, Group group);

        bool CanControlUsers(User user, Group group);
    }
}
