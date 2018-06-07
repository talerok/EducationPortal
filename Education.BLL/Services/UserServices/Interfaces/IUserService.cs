using System;
using System.Collections.Generic;
using System.Text;
using Education.BLL.DTO.User;
using System.Security.Claims;
using Education.BLL.DTO;

namespace Education.BLL.Services.UserServices.Interfaces
{
    public enum AuthStatus
    {
        Succsess,
        UserNotFound,
        UserBanned,
        WrongKey,
        NeedNewKey,
        KeySent,
        InternalError
    }

    public interface IUserService
    {
        IClaimService ClaimService { get; }
        AuthResult Login(LoginInfoDTO loginInfoDTO);
        RegisterResult Register(RegUserInfo user);
        void Logout(IEnumerable<Claim> claims);
        void ResetClaims(UserDTO userDTO);

    }
}
