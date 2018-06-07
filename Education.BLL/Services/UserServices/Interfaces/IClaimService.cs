using Education.BLL.DTO.User;
using Education.DAL.Entities;
using Education.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Education.BLL.Services.UserServices.Interfaces
{
    public interface IClaimService
    {
        ClaimsIdentity Generate(User user, IUOW Data, LoginInfoDTO loginInfoDTO);
        IEnumerable<ClaimInfoDTO> GetInfo(User user, IUOW Data);
        void RemoveAllClaims(User user, IUOW Data, params string[] without);
        void Logout(IEnumerable<Claim> claims);
        UserDTO GetUser(IEnumerable<Claim> claims);

    }
}
