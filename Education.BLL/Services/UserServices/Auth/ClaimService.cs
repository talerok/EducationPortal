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

        class ClaimNames
        { 
            public static string UserID = "UserId";
            public static string SessionID = "SessionID";
            public static string SessionValue = "SessionValue";
        }
        public ClaimService(IUOWFactory uowf)
        {
            DataFactory = uowf;
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

        private UserDTO Generate(IEnumerable<Claim> claims)
        {
            var idClaim = claims.FirstOrDefault(x => x.Type == ClaimNames.UserID);
            var sIdClaim = claims.FirstOrDefault(x => x.Type == ClaimNames.SessionID);
            if (idClaim == null || sIdClaim == null) return null;
            return new UserDTO { Id = int.Parse(idClaim.Value), ClaimId = int.Parse(sIdClaim.Value) };
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
            try
            {
                return Generate(claims);
            }
            catch
            {
                return null;
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
                new Claim(ClaimNames.UserID, user.Id.ToString()),
                new Claim(ClaimNames.SessionID, claim.Id.ToString()),
                new Claim(ClaimNames.SessionValue, claim.Value.ToString()),
            };
            return new ClaimsIdentity(claims, "ApplicationCookie");
         
        }

        public void Logout(IEnumerable<Claim> claims)
        {
            if (claims == null) return;
            using (var Data = DataFactory.Get())
            {
                var a = claims.FirstOrDefault(x => x.Type == ClaimNames.SessionValue);
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