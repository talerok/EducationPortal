using Education.BLL.Logic.Interfaces;
using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Logic.Rules
{
    public class MessageRules : IMessageRules
    {
        private IThemeRules ThemeRules;
        private ISectionRules SectionRules;
        public MessageRules(IThemeRules themeRules, ISectionRules sectionRules)
        {
            ThemeRules = themeRules;
            SectionRules = sectionRules;
        }

        public bool CanCreate(User user, Theme theme)
        {
            if (user == null) return false;
            if (theme == null) throw new ArgumentNullException("theme");
            if (theme.Open && ThemeRules.CanRead(user, theme)) return true;
            else if (!theme.Open && SectionRules.CanEdit(user, theme.Section)) return true;
            else return false;
        }

        public bool CanDelete(User user, Message message)
        {
            return CanEdit(user, message);
        }

        public bool CanEdit(User user, Message message)
        {
            if (user == null) return false;
            if (message == null) throw new ArgumentNullException("theme");
            if (message.Theme == null)
                throw new NullReferenceException("message.Theme must be not null");
            if (message.Theme.Open && message.Owner.Id == user.Id && ThemeRules.CanRead(user, message.Theme))
                return true;
            else if (SectionRules.CanEdit(user, message.Theme.Section))
                return true;
            else return false;
        }

        public bool CanRead(User user, Message message)
        {
            return ThemeRules.CanRead(user, message.Theme);
        }
    }
}
