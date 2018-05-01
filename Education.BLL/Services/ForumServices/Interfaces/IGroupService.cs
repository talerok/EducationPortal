using Education.BLL.DTO.Forum;
using Education.BLL.DTO.Forum.Edit;
using Education.BLL.DTO.User;
using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.ForumServices.Interfaces
{
    public interface IGroupService : IControlService<GroupDTO, GroupEditDTO> 
    {
        UserGroupInfoDTO GetUsers(int groupId, UserDTO userDTO);
        AccessCode ChangeUserRole(int groupId, int userId, UserGroupStatus status, UserDTO userDTO);
        AccessCode RemoveUser(int groupId, int userId, UserDTO userDTO);
        AccessCode Request(UserDTO userDTO, int GroupId);
        AccessCode Leave(UserDTO userDTO, int GroupId);
        bool CanCreate(UserDTO userDTO);
    }
}
