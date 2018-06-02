using Education.BLL.DTO;
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
        private IDTOHelper DTOHelper;
        private static int MessagesPerPage = 10;
        private SectionRoute GetRoute(Section section)
        {
            if (section.Group == null) return null;
            return new SectionRoute
            {
                GroupId = section.Group.Id,
                GroupName = section.Group.Name
            };
        }

        private ThemeRoute GetRoute(Theme theme)
        {
            if (theme.Section == null) return null;
            return new ThemeRoute
            {
                SectionRoute = GetRoute(theme.Section),
                SectionId = theme.Section.Id,
                SectionName = theme.Section.Name
            };
        }

        private MessageRoute GetRoute(Message message)
        {
            if (message.Theme == null) return null;
            return new MessageRoute
            {
                ThemeRoute = GetRoute(message.Theme),
                ThemeId = message.Theme.Id,
                ThemeName = message.Theme.Name
            };
        }

        public ForumDTOHelper(IMessageRules messageRules, IThemeRules themeRules, ISectionRules sectionRules, IGroupRules groupRules, IDTOHelper dtoHelper)
        {
            MessageRules = messageRules;
            ThemeRules = themeRules;
            SectionRules = sectionRules;
            GroupRules = groupRules;
            DTOHelper = dtoHelper;
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
                Access = new GroupAccessDTO
                {
                    CanCreateElements = SectionRules.CanCreate(user, group),
                    CanDelete = GroupRules.CanDelete(user, group),
                    CanRead = GroupRules.CanRead(user, group),
                    CanUpdate = GroupRules.CanEdit(user, group),
                    CanControlUsers = GroupRules.CanControlUsers(user, group)
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
                    Pages = GetThemePages(theme),
                    Name = theme.Name,
                    Owner = DTOHelper.GetUser(theme?.Messages?.FirstOrDefault()?.Owner),
                    LastMessages = GetDTO(theme?.Messages?.LastOrDefault(), user)
                });
            return new SectionDTO
            {
                Id = section.Id,
                Name = section.Name,
                Open = section.Open,
                Themes = themes,
                Route = GetRoute(section),
                Access = new FullAccessDTO
                {
                    CanCreateElements = ThemeRules.CanCreate(user, section),
                    CanDelete = SectionRules.CanDelete(user, section),
                    CanRead = SectionRules.CanRead(user, section),
                    CanUpdate = SectionRules.CanEdit(user, section)
                }
            };
        }
        
        private ThemeDTO CreateThemeDTO(Theme theme, User user, IEnumerable<Message> messages)
        {
            var mres = new List<MessageDTO>();
            foreach (var message in messages) mres.Add(GetDTO(message, user));
            return new ThemeDTO
            {
                Name = theme.Name,
                Open = theme.Open,
                Messages = mres,
                Id = theme.Id,
                Route = GetRoute(theme),
                Access = new FullAccessDTO
                {
                    CanCreateElements = MessageRules.CanCreate(user, theme),
                    CanDelete = ThemeRules.CanDelete(user, theme),
                    CanRead = ThemeRules.CanRead(user, theme),
                    CanUpdate = ThemeRules.CanEdit(user, theme)
                }
            };
        }

        private int GetThemePages(Theme theme)
        {
            int res = theme.Messages.Count / MessagesPerPage;
            if (theme.Messages.Count % MessagesPerPage != 0) res++;
            return res;
        }

        private IEnumerable<Message> GetMessages(Theme theme, int page = 0)
        {
            var messages = theme.Messages.OrderBy(x => x.Time);
            if (page == 0) return messages;
            else return messages.Skip((page - 1) * MessagesPerPage)
                .Take(MessagesPerPage);
        }

        public ThemeDTO GetDTO(Theme theme, User user, int page)
        {
            if (theme == null) return null;
            if (page < 1) page = 1;
            var res = CreateThemeDTO(theme, user, GetMessages(theme, page));
            res.Pages = GetThemePages(theme);
            res.CurPage = page;
            return res;
        }

        public ThemeDTO GetDTO(Theme theme, User user)
        {
            if (theme == null) return null;
            var res = CreateThemeDTO(theme, user, GetMessages(theme));
            res.Pages = 1;
            res.CurPage = 1;
            return res;
        }

        public MessageDTO GetDTO(Message message, User user)
        {
            if (message == null) return null;
            return new MessageDTO
            {
                LastEditor = DTOHelper.GetUser(message.LastEditor),
                LastEditTime = message.LastEditTime,
                Owner = DTOHelper.GetUser(message.Owner),
                Time = message.Time,
                Text = message.Text,
                Id = message.Id,
                Route = GetRoute(message),
                Access = new AccessDTO
                {
                    CanDelete = MessageRules.CanDelete(user, message),
                    CanRead = MessageRules.CanRead(user, message),
                    CanUpdate = MessageRules.CanEdit(user, message)
                }

            };
        }

        public UserGroupDTO GetDTO(UserGroup userGroup)
        {
            return new UserGroupDTO
            {
                UserInfo = DTOHelper.GetUser(userGroup.User),
                Status = userGroup.Status
            };
        }

        public GroupPreviewDTO GetDTO(User user, Group group)
        {
            UserGroupInfo info = null;
            
            if(user != null)
            {
                info = new UserGroupInfo();
                var usergroup = group.Users.FirstOrDefault(x => x.UserId == user.Id);
                if (usergroup != null)
                {
                    info.Member = true;
                    info.Status = usergroup.Status;
                }
                else info.Member = false;
            }
            return new GroupPreviewDTO
            {
                Id = group.Id,
                Logo = group.Logo,
                Name = group.Name,
                Open = group.Open,
                Status = info,
                Access = new GroupAccessDTO
                {
                    CanCreateElements = SectionRules.CanCreate(user, group),
                    CanDelete = GroupRules.CanDelete(user, group),
                    CanRead = GroupRules.CanRead(user, group),
                    CanUpdate = GroupRules.CanEdit(user, group),
                    CanControlUsers = GroupRules.CanControlUsers(user,group)
                }
            };
        }

        public MessagePreviewDTO GetDTO(Message message)
        {
            var res = GetMessages(message.Theme).ToList();
            int page = res.IndexOf(message) / MessagesPerPage + 1;
            return new MessagePreviewDTO { Page = page, ThemeId = message.Theme.Id };
        }
    }
}
