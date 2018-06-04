using Education.DAL.Entities;
using Education.DAL.Entities.Pages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Logic.Interfaces
{
    public interface INoteRules
    {
        bool CanCreate(User user);
        bool CanEdit(User user, Note note);
        bool CanDelete(User user, Note note);
        bool CanRead(User user, Note note);
    }
}
