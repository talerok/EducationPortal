using Education.BLL.Logic.Interfaces;
using Education.DAL.Entities;
using Education.DAL.Entities.Pages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Logic.Rules
{
    public class PageRules : IPageRules
    {
        public bool CanCreate(User user)
        {
            return user != null && user.Level == 2;
        }

        public bool CanDelete(User user, Page page)
        {
            return CanCreate(user);
        }

        public bool CanEdit(User user, Page page)
        {
            return user != null && user.Level > 0;
        }

        public bool CanRead(User user, Page page)
        {
            return page.Published || CanEdit(user, page);
        }
    }
}
