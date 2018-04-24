using Education.BLL.Logic.Interfaces;
using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Education.BLL.Logic.Rules
{
    public class ThemeRules : IThemeRules
    {
        private ISectionRules SectionRules;

        private bool IsOwner(Theme theme, User user)
        {
            var firstMessage = theme?.Messages?.FirstOrDefault();
            if (firstMessage == null || firstMessage.Owner.Id != user.Id) return false;
            else return true;
        }

        public ThemeRules(ISectionRules sectionRules)
        {
            SectionRules = sectionRules;
        }

        public bool CanCreate(User user, Section section)
        {
            if (section == null) throw new ArgumentNullException("section");
            if (user == null) return false;
            if (section == null) throw new ArgumentNullException("section");
            if (section.Open && SectionRules.CanRead(user, section)) return true;
            else if (!section.Open && SectionRules.CanEdit(user, section)) return true;
            else return false;
        }

        public bool CanDelete(User user, Theme theme)
        {
            return SectionRules.CanEdit(user, theme.Section);
        }

        public bool CanEdit(User user, Theme theme)
        {
            if (theme == null) throw new ArgumentNullException("theme");
            if (user == null) return false;
            if (IsOwner(theme, user)) return true;
            if (SectionRules.CanEdit(user, theme.Section)) return true;
            else return false;
        }

        public bool CanRead(User user, Theme theme)
        {
            return SectionRules.CanRead(user, theme.Section);
        }
    }
}
