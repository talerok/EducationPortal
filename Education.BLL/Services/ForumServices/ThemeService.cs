using Education.BLL.DTO.Forum;
using Education.BLL.DTO.User;
using Education.BLL.Logic.Interfaces;
using Education.BLL.Services.ForumServices.Interfaces;
using Education.DAL.Entities;
using Education.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Education.BLL.Services.ForumServices
{
    public class ThemeService : IThemeService
    {
        private IThemeRules ThemeRules;
        private IGetUserDTO GetUserService;
        private IUOWFactory DataFactory;
        private IForumDTOHelper forumDTOHelper;

        public ThemeService(IThemeRules rules, IGetUserDTO getUserDTO, IUOWFactory dataFactory, IForumDTOHelper forumHelper)
        {
            GetUserService = getUserDTO;
            DataFactory = dataFactory;
            forumDTOHelper = forumHelper;
            ThemeRules = rules;
        }

        private (AccessCode Code,Theme Theme, User User) CheckTheme(UserDTO userDTO, int ThemeId, Func<User,Theme,bool> checkFunc, IUOW Data)
        {
            var user = GetUserService.Get(userDTO, Data);
            var theme = Data.ThemeRepository.Get().FirstOrDefault(x => x.Id == ThemeId);
            if (theme == null) return (AccessCode.NotFound,null,user);
            if (checkFunc(user, theme)) return (AccessCode.Succsess,theme,user);
            else return (AccessCode.NoPremision,null,user);
        }

        private void EditTheme(Theme theme, ThemeDTO themeDTO)
        {
            theme.Name = themeDTO.Name;
            theme.Open = themeDTO.Open;
        }

        public CreateResultDTO Create(ThemeDTO DTO, UserDTO userDTO)
        {
            using(var Data = DataFactory.Get())
            {
                var user = GetUserService.Get(userDTO,Data);
                var section = Data.SectionRepository.Get().FirstOrDefault(x => x.Id == DTO.SectionId);
                if (section == null) return CreateResultDTO.NotFound;
                if (DTO.Messages == null || DTO.Messages.First() == null) return CreateResultDTO.Error;
                if (ThemeRules.CanCreate(user, section))
                {
                    var theme = new Theme();
                    EditTheme(theme, DTO);
                    theme.Section = section;
                    //---------------------
                    var firstMessage = DTO.Messages.First();
                    theme.Messages.Add(
                        new Message {
                            Owner = user,
                            Text = firstMessage.Text,
                            Theme = theme,
                            Time = DateTime.Now
                        }
                    );
                    //--------------------
                    Data.ThemeRepository.Add(theme);
                    Data.SaveChanges();
                    return new CreateResultDTO(theme.Id, AccessCode.Succsess);
                }
                else return CreateResultDTO.NoPremision;
            }
        }

        public AccessCode Delete(int id, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var check = CheckTheme(userDTO, id, ThemeRules.CanDelete, Data);
                if(check.Code == AccessCode.Succsess)
                {
                    Data.ThemeRepository.Delete(check.Theme);
                    Data.SaveChanges();
                }
                return check.Code;
            }
        }

        public (AccessCode, ThemeDTO) Read(int id, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var check = CheckTheme(userDTO, id, ThemeRules.CanRead, Data);
                if (check.Code == AccessCode.Succsess)
                    return (check.Code, forumDTOHelper.GetDTO(check.Theme, check.User));
                else return (check.Code, null);
            }
        }

        public AccessCode Update(ThemeDTO DTO, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var check = CheckTheme(userDTO, DTO.Id, ThemeRules.CanEdit, Data);
                if (check.Code == AccessCode.Succsess)
                {
                    EditTheme(check.Theme,DTO);
                    Data.ThemeRepository.Edited(check.Theme);
                    Data.SaveChanges();
                }
                return check.Code;
            }
        }
    }
}
