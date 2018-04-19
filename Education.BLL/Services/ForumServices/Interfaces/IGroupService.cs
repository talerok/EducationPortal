using Education.BLL.DTO.Forum;
using Education.BLL.DTO.User;
using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.ForumServices.Interfaces
{


    interface IGroupService : IControlService<GroupDTO>
    {
        ControlResult SendRequest(int Id, UserDTO userDTO);
        ControlResult RemoveUser(int UserId, int GroupId, UserDTO userDTO);
        ControlResult ChangeUserStatus(int UserID, int GroupId, UserGroupStatus newStatus, UserDTO userDTO);
    }
}
