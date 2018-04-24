using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Logic.Interfaces
{
    public interface IMessageRules
    {
        bool CanCreate(User user, Theme theme);

        bool CanEdit(User user, Message message);

        bool CanDelete(User user, Message message);

        bool CanRead(User user, Message message);
    }
}
