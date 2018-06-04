using Education.DAL.Entities;
using Education.DAL.Entities.Pages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Logic.Interfaces
{
    public interface IPageRules
    {
        bool CanCreate(User user);
        bool CanEdit(User user, Page page);
        bool CanDelete(User user, Page page);
        bool CanRead(User user, Page page);
    }
}
