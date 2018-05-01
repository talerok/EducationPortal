using Education.BLL.DTO.Forum;
using Education.BLL.DTO.Forum.Edit;
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
    public class SectionService : ISectionService
    {
        private ISectionRules SectionRules;
        private IGetUserDTO GetUserService;
        private IUOWFactory DataFactory;
        private IForumDTOHelper forumDTOHelper;

        public SectionService(ISectionRules rules, IGetUserDTO getUserDTO, IUOWFactory dataFactory, IForumDTOHelper forumHelper)
        {
            GetUserService = getUserDTO;
            DataFactory = dataFactory;
            forumDTOHelper = forumHelper;
            SectionRules = rules;
        }

        private (AccessCode Code, Section Section, User User) CheckSection(UserDTO userDTO, int SectionId, Func<User, Section, bool> checkFunc, IUOW Data)
        {
            var user = GetUserService.Get(userDTO, Data);
            var section = Data.SectionRepository.Get().FirstOrDefault(x => x.Id == SectionId);
            if (section == null) return (AccessCode.NotFound, null, user);
            if (checkFunc(user, section)) return (AccessCode.Succsess, section, user);
            else return (AccessCode.NoPremision, null, user);
        }

        private void EditSection(SectionEditDTO sectionDTO, Section section)
        {
            section.Name = sectionDTO.Name;
            section.Open = sectionDTO.Open;
        }

        public CreateResultDTO Create(SectionEditDTO DTO, UserDTO userDTO)
        {
            using(var Data = DataFactory.Get())
            {
                var user = GetUserService.Get(userDTO, Data);
                var group = Data.GroupRepository.Get().FirstOrDefault(x => x.Id == DTO.GroupId);
                if (group == null) return CreateResultDTO.NotFound;
                if (SectionRules.CanCreate(user, group))
                {
                    var section = new Section();
                    EditSection(DTO, section);
                    section.Group = group;
                    Data.SectionRepository.Add(section);
                    Data.SaveChanges();
                    return new CreateResultDTO(section.Id, AccessCode.Succsess);
                }
                else return CreateResultDTO.NoPremision;
            }
        }

        public AccessCode Delete(int id, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var check = CheckSection(userDTO, id, SectionRules.CanDelete, Data);
                if (check.Code == AccessCode.Succsess)
                {
                    Data.SectionRepository.Delete(check.Section);
                    Data.SaveChanges();
                }
                return check.Code;
            }
        }

        public (AccessCode, SectionDTO) Read(int id, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var check = CheckSection(userDTO, id, SectionRules.CanRead, Data);
                if (check.Code == AccessCode.Succsess)
                    return (check.Code, forumDTOHelper.GetDTO(check.Section, check.User));
                else return (check.Code, null);
            }
        }

        public AccessCode Update(SectionEditDTO DTO, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var check = CheckSection(userDTO, DTO.Id, SectionRules.CanEdit, Data);
                if (check.Code == AccessCode.Succsess)
                {
                    EditSection(DTO, check.Section);
                    Data.SectionRepository.Edited(check.Section);
                    Data.SaveChanges();
                }
                return check.Code;
            }
        }

        public bool CanCreate(int GroupId, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var user = GetUserService.Get(userDTO, Data);
                var group = Data.GroupRepository.Get().FirstOrDefault(x => x.Id == GroupId);
                if (group == null) return false;
                return SectionRules.CanCreate(user, group);
            }
        }
    }
}
