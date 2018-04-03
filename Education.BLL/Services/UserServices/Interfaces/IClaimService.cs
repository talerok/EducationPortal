using Education.BLL.DTO.User;
using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Education.BLL.Services.UserServices.Interfaces
{
    public interface IClaimService
    {
        ClaimsIdentity Generate(User user, LoginInfoDTO loginInfoDTO);

        IEnumerable<ClaimInfoDTO> GetInfo(User user);

        void RemoveAllClaims(User user, params string[] without);
        void Logout(IEnumerable<Claim> claims);
        UserDTO GetUser(IEnumerable<Claim> claims);

    }
}
