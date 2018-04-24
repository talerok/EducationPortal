using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Logic.Interfaces
{
    public interface IThemeRules
    {
        bool CanCreate(User user, Section section);

        bool CanEdit(User user, Theme theme);

        bool CanDelete(User user, Theme theme);

        bool CanRead(User user, Theme theme);
    }
}
