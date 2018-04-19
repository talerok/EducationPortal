using Education.BLL.DTO.User;
using Education.BLL.Services.UserServices.Interfaces;
using Education.DAL.Entities;
using Education.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Education.BLL.Services.UserServices.Auth
{
    public class ClaimService : IClaimService
    {
        private IUOWFactory DataFactory;

        public ClaimService(IUOWFactory uowf)
        {
            DataFactory = uowf;
        }

        public User GetUser(UserDTO userDTO, IUOW Data)
        {
            var login = userDTO.Login.ToLower();
            var email = userDTO.Email.ToLower();
            var phone = userDTO.PhoneNumber.ToLower();
            return Data.UserRepository.Get().FirstOrDefault(x => x.Login == login
            && x.Password == userDTO.Password);
        }

        private User GetUser(LoginInfoDTO loginInfoDTO, IUOW Data)
        {
            var name = loginInfoDTO.Login.ToLower();
            return Data.UserRepository.Get().FirstOrDefault(x => x.Login == name
            && x.Password == loginInfoDTO.Password);
        }

        private UserClaim GenerateClaim(User user, LoginInfoDTO loginInfoDTO)
        {
            var now = DateTime.Now;
            var claim = new UserClaim {
                LoginBrowser = loginInfoDTO.Browser,
                User = user,
                LoginTime = DateTime.Now,
                LoginIp = loginInfoDTO.IP
            };
            claim.Value = user.Login + "-" + now.ToString();
            return claim;
        }

        private User Generate(IEnumerable<Claim> claims, IUOW Data)
        {
            var a = claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType);
            if (a == null) return null;
            var claim = Data.UserClaimRepository.Get().FirstOrDefault(x => x.Value == a.Value);
            if (claim == null) return null;
            return claim.User;
        }

        public void RemoveAllClaims(User user, IUOW Data, params string[] without)
        {
            if (user == null) return;
            var claims = Data.UserClaimRepository.Get().Where(x => x.User == user).ToList();
            foreach (var claim in without)
            {
                var res = claims.FirstOrDefault(x => x.Value == claim);
                if (res != null) claims.Remove(res);
            }

            Data.UserClaimRepository.Delete(claims);
            Data.SaveChanges();
       
        }

        //---------------------------------------------------
        public UserDTO GetUser(IEnumerable<Claim> claims)
        {
            using (var Data = DataFactory.Get())
            {
                var user = Generate(claims, Data);
                if (user == null) return null;
                if (user.Ban != null && DateTime.Now < user.Ban.EndTime) return null;
                var userDTO = new UserDTO
                {
                    Id = user.Id,
                    Login = user.Login,
                    Email = user.Email?.Value,
                    PhoneNumber = user.Phone?.Value,
                    FullName = user.Info?.FullName,
                    Password = user.Password
                };
                return userDTO;
            }
        }
        //---------------------------------------------------
        public ClaimsIdentity Generate(User user, IUOW Data, LoginInfoDTO loginInfoDTO)
        {
             if (user == null || loginInfoDTO == null) return null;
            var claim = GenerateClaim(user, loginInfoDTO);
            Data.UserClaimRepository.Add(claim);
            Data.SaveChanges();
            var claims = new List<Claim>
            {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, claim.Value),
            };
            return new ClaimsIdentity(claims, "ApplicationCookie");
         
        }

        public void Logout(IEnumerable<Claim> claims)
        {
            if (claims == null) return;
            using (var Data = DataFactory.Get())
            {
                var a = claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType);
                var claim = Data.UserClaimRepository.Get().FirstOrDefault(x => x.Value == a.Value);
                if (claim == null) return;
                Data.UserClaimRepository.Delete(claim);
                Data.SaveChanges();
            }
        }

        public IEnumerable<ClaimInfoDTO> GetInfo(User user, IUOW Data)
        {
            if (user == null) return null;

            var claims = Data.UserClaimRepository.Get().Where(x => x.User == user);
            var result = new List<ClaimInfoDTO>();
            foreach (var claim in claims)
                result.Add(new ClaimInfoDTO { Browser = claim.LoginBrowser, Ip = claim.LoginIp, LoginTime = claim.LoginTime });
            return result;
    
        }

    }
}