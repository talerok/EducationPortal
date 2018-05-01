using Education.BLL.DTO.Forum;
using Education.BLL.DTO.Forum.Edit;
using Education.BLL.DTO.User;
using Education.BLL.Logic.Interfaces;
using Education.BLL.Services.ForumServices.Interfaces;
using Education.BLL.Services.UserServices.Interfaces;
using Education.DAL.Entities;
using Education.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Education.BLL.Services.ForumServices
{
    public class GroupService : IGroupService
    {
        private IGetUserDTO getUserService;
        private IUOWFactory DataFactory;
        private IForumDTOHelper ForumDTOHelper;
        private IGroupRules GroupRule;

        private (AccessCode Code, Group Group, User User) CheckRule(UserDTO userDTO, int GroupId, Func<User, Group, bool> checkFunc, IUOW Data)
        {
            var user = getUserService.Get(userDTO, Data);
            var group = Data.GroupRepository.Get().FirstOrDefault(x => x.Id == GroupId);
            if (group == null) return (AccessCode.NotFound, null, user);
            if (checkFunc(user, group)) return (AccessCode.Succsess, group, user);
            else return (AccessCode.NoPremision, null, user);
        }

        private void EditGroup(GroupEditDTO groupDTO, Group group)
        {
            group.Logo = groupDTO.Logo;
            group.Name = groupDTO.Name;
            group.Open = groupDTO.Open;             
        }

        public GroupService(IGroupRules rules, IGetUserDTO getUserDTO, IUOWFactory dataFactory, IForumDTOHelper forumDTOHelper)
        {
            getUserService = getUserDTO;
            DataFactory = dataFactory;
            ForumDTOHelper = forumDTOHelper;
            GroupRule = rules;
        }
        
        public CreateResultDTO Create(GroupEditDTO groupDTO, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var user = getUserService.Get(userDTO, Data);
                if (GroupRule.CanCreate(user))
                {
                    var group = new Group();
                    EditGroup(groupDTO,group);
                    Data.GroupRepository.Add(group);
                    Data.SaveChanges();
                    return new CreateResultDTO(group.Id, AccessCode.Succsess);
                }
            }
            return CreateResultDTO.NoPremision;
        }

        public AccessCode Update(GroupEditDTO groupDTO, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var check = CheckRule(userDTO, groupDTO.Id, GroupRule.CanEdit, Data);
                if (check.Code == AccessCode.Succsess)
                {
                    EditGroup(groupDTO, check.Group);
                    Data.GroupRepository.Edited(check.Group);
                    Data.SaveChanges();
                }
                return check.Code;
            }
        }

        public AccessCode Delete(int id, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var check = CheckRule(userDTO, id, GroupRule.CanDelete, Data);
                if (check.Code == AccessCode.Succsess)
                {
                    Data.GroupRepository.Delete(check.Group);
                    Data.SaveChanges();
                }
                return check.Code;
            }
        }

        public  (AccessCode,GroupDTO) Read(int id, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var check = CheckRule(userDTO, id, GroupRule.CanRead, Data);
                if (check.Code == AccessCode.Succsess)
                    return (check.Code, ForumDTOHelper.GetDTO(check.Group, check.User));
                else return (check.Code, null);
            }
        }

        public AccessCode Request(UserDTO userDTO, int GroupId)
        {
            using(var Data = DataFactory.Get())
            {
                var user = getUserService.Get(userDTO, Data);
                if (user == null) return AccessCode.NoPremision;
                var group = Data.GroupRepository.Get().FirstOrDefault(x => x.Id == GroupId);
                if (group == null) return AccessCode.NotFound;
                if (group.Open) return AccessCode.NoPremision;
                var usergroup = group.Users.FirstOrDefault(x => x.UserId == user.Id);
                if (usergroup != null) return AccessCode.Error;
                Data.UserGroupRepository.Add(new UserGroup
                {
                    Group = group,
                    User = user,
                    Status = UserGroupStatus.request
                });
                Data.SaveChanges();
                return AccessCode.Succsess;
            }
        }

        public AccessCode Leave(UserDTO userDTO, int GroupId)
        {
            using (var Data = DataFactory.Get())
            {
                var user = getUserService.Get(userDTO, Data);
                if (user == null) return AccessCode.NoPremision;
                var group = Data.GroupRepository.Get().FirstOrDefault(x => x.Id == GroupId);
                if (group == null) return AccessCode.NotFound;
                if (group.Open) return AccessCode.NoPremision;
                var usergroup = group.Users.FirstOrDefault(x => x.UserId == user.Id);
                if (usergroup == null) return AccessCode.Error;
                Data.UserGroupRepository.Delete(usergroup);
                Data.SaveChanges();
                return AccessCode.Succsess;
            }
        }

        public UserGroupInfoDTO GetUsers(int groupId, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var check = CheckRule(userDTO, groupId, GroupRule.CanControlUsers, Data);
                if (check.Code == AccessCode.Succsess)
                {
                    return new UserGroupInfoDTO
                    {
                        Code = AccessCode.Succsess,
                        Data = check.Group.Users.Select(x => ForumDTOHelper.GetDTO(x)).ToList()
                    };
                }
                else if (check.Code == AccessCode.NoPremision) return UserGroupInfoDTO.NoPremision;
                else if (check.Code == AccessCode.NotFound) return UserGroupInfoDTO.NotFound;
                else return UserGroupInfoDTO.Error;
            }
        }

        public AccessCode ChangeUserRole(int groupId, int userId, UserGroupStatus status, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var check = CheckRule(userDTO, groupId, GroupRule.CanControlUsers, Data);
                if (check.Code == AccessCode.Succsess)
                {
                    var userGroup = check.Group.Users.FirstOrDefault(x => x.UserId == userId);
                    if (userGroup == null) return AccessCode.NotFound;
                    else userGroup.Status = status;
                    Data.UserGroupRepository.Edited(userGroup);
                    Data.SaveChanges();
                }
                return check.Code;
            }
        }

        public AccessCode RemoveUser(int groupId, int userId, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var check = CheckRule(userDTO, groupId, GroupRule.CanControlUsers, Data);
                if (check.Code == AccessCode.Succsess)
                {
                    var userGroup = check.Group.Users.FirstOrDefault(x => x.UserId == userId);
                    if (userGroup == null) return AccessCode.NotFound;
                    Data.UserGroupRepository.Delete(userGroup);
                    Data.SaveChanges();
                }
                return check.Code;
            }
        }

        public bool CanCreate(UserDTO userDTO)
        {
            using(var Data = DataFactory.Get())
            {
                var user = getUserService.Get(userDTO,Data);
                return GroupRule.CanCreate(user);
            }
        }
    }
}
