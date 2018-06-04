using Education.BLL.Logic.Interfaces;
using Education.DAL.Entities;
using Education.DAL.Entities.Pages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Logic.Rules
{
    public class NoteRules : INoteRules
    {
        public bool CanCreate(User user)
        {
            return user != null && user.Level > 0;
        }

        public bool CanDelete(User user, Note note)
        {
            return CanCreate(user);
        }

        public bool CanEdit(User user, Note note)
        {
            return CanCreate(user);
        }

        public bool CanRead(User user, Note note)
        {
            return note.Published || CanEdit(user, note);
        }
    }
}
