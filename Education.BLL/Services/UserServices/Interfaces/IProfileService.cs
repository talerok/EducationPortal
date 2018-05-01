using Education.BLL.DTO.User;
using Education.BLL.DTO;
using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.UserServices.Interfaces
{
    public enum SetContactCode
    {
        Succsess,
        UserNotFound,
        AlreadySet,
        AlreadyExists,
        WrongValue
    };

    public interface IProfileService
    {
        IClaimService ClaimService { get; }
        ConfirmResult ResetAuthType(UserDTO userDTO, string key = null);
        ConfirmResult SetAuthType(UserDTO userDTO, AuthType authType, string key = null);
        ConfirmResult SetPassword(UserDTO userDTO, string oldpassword, string newPassword, string key = null);
        UserProfileDTO GetUserProfile(UserDTO userDTO);
        ConfirmResult ConfirmEmail(UserDTO userDTO, string key = null);
        ConfirmResult ConfirmPhone(UserDTO userDTO, string key = null);
        ConfirmResult RemoveEmail(UserDTO userDTO, string key = null);
        ConfirmResult RemovePhone(UserDTO userDTO, string key = null);
        SetContactCode SetEmail(UserDTO userDTO, string email);
        SetContactCode SetPhone(UserDTO userDTO, string phone);
        ConfirmCode SetAvatar(UserDTO userDTO, string path);
    }
}
