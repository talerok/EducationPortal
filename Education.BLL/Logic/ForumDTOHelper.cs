using Education.BLL.DTO.Forum;
using Education.BLL.Logic.Interfaces;
using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Education.BLL.Logic
{
    public class ForumDTOHelper : IForumDTOHelper
    {
        private IMessageRules MessageRules;
        private IThemeRules ThemeRules;
        private ISectionRules SectionRules;
        private IGroupRules GroupRules;

        public ForumDTOHelper(IMessageRules messageRules, IThemeRules themeRules, ISectionRules sectionRules, IGroupRules groupRules)
        {
            MessageRules = messageRules;
            ThemeRules = themeRules;
            SectionRules = sectionRules;
            GroupRules = groupRules;
        }

        private UserPublicInfoDTO GetUser(User user)
        {
            return new UserPublicInfoDTO
            {
                AvatarPath = user.Info.Avatar,
                Id = user.Id,
                Name = user.Info.FullName
            };
        }

        public GroupDTO GetDTO(Group group, User user)
        {
            if (group == null) return null;
            var sections = new List<SectionDTO>();
            foreach(var Section in group.Sections)
                sections.Add(GetDTO(Section, user));
            return new GroupDTO
            {
                Logo = group.Logo,
                Name = group.Name,
                Open = group.Open,
                Id = group.Id,
                Access = new AccessDTO
                {
                    CanCreateElements = SectionRules.CanCreate(user, group),
                    CanDelete = GroupRules.CanDelete(user, group),
                    CanRead = GroupRules.CanRead(user, group),
                    CanUpdate = GroupRules.CanEdit(user, group)
                },
                Sections = sections
            };
        }

        public SectionDTO GetDTO(Section section, User user)
        {
            if (section == null) return null;
            var themes = new List<ThemePreviewDTO>();
            foreach (var theme in section.Themes)
                themes.Add(new ThemePreviewDTO
                {
                    Id = theme.Id,
                    Name = theme.Name,
                    Owner = GetUser(theme?.Messages?.FirstOrDefault()?.Owner),
                    LastMessages = GetDTO(theme?.Messages?.LastOrDefault(), user)
                });
            return new SectionDTO
            {
                GroupId = section.Group.Id,
                Id = section.Id,
                Name = section.Name,
                Open = section.Open,
                Themes = themes,
                Access = new AccessDTO
                {
                    CanCreateElements = ThemeRules.CanCreate(user, section),
                    CanDelete = SectionRules.CanDelete(user, section),
                    CanRead = SectionRules.CanRead(user, section),
                    CanUpdate = SectionRules.CanEdit(user, section)
                }
            };
        }
        
        public ThemeDTO GetDTO(Theme theme, User user)
        {
            if (theme == null) return null;
            var messages = new List<MessageDTO>();
            foreach (var message in theme.Messages) messages.Add(GetDTO(message, user));
            return new ThemeDTO
            {
                Name = theme.Name,
                Open = theme.Open,
                SectionId = theme.Section.Id,
                Messages = messages,
                Id = theme.Id,
                Access = new AccessDTO
                {
                    CanCreateElements = MessageRules.CanCreate(user, theme),
                    CanDelete = ThemeRules.CanDelete(user, theme),
                    CanRead = ThemeRules.CanRead(user, theme),
                    CanUpdate = ThemeRules.CanEdit(user, theme)
                }
            };
        }

        public MessageDTO GetDTO(Message message, User user)
        {
            if (message == null) return null;
            return new MessageDTO
            {
                LastEditor = GetUser(message.LastEditor),
                LastEditTime = message.LastEditTime,
                Owner = GetUser(message.Owner),
                Time = message.Time,
                ThemeId = message.Theme.Id,
                Text = message.Text,
                Id = message.Id,
                Access = new AccessDTO
                {
                    CanCreateElements = false,
                    CanDelete = MessageRules.CanDelete(user, message),
                    CanRead = MessageRules.CanRead(user, message),
                    CanUpdate = MessageRules.CanEdit(user, message)
                }

            };
        }
    }
}
