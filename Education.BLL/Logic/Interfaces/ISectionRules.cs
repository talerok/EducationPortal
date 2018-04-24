using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Logic.Interfaces
{
    public interface ISectionRules
    {
        bool CanCreate(User user, Group group);

        bool CanEdit(User user, Section section);

        bool CanDelete(User user, Section section);

        bool CanRead(User user, Section section);
    }
}
