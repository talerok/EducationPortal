using Education.BLL.DTO.Forum;
using Education.BLL.DTO.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.AdminService.Interfaces
{
    public interface IAdminService
    {
        (AccessCode Code, IEnumerable<AdminUserInfoDTO> Result) Search(UserDTO userDTO, string name);
        (AccessCode Code, IEnumerable<AdminUserInfoDTO> Result) GetAll(UserDTO userDTO);
        (AccessCode Code, AdminUserInfoDTO Result) GetUser(UserDTO userDTO, int id);
        AccessCode EditUser(UserDTO userDTO, AdminUserInfoDTO userInfo);
        AccessCode ResetClaims(UserDTO userDTO, int UserId);
        bool IsAdmin(UserDTO userDTO);
    }
}
