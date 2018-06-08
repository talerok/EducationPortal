using Education.BLL.DTO.Forum;
using Education.BLL.DTO.User;
using Education.BLL.Logic.Interfaces;
using Education.BLL.Services.AdminService.Interfaces;
using Education.DAL.Entities;
using Education.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Education.BLL.Services.AdminService
{
    public class AdminService : IAdminService
    {
        private IGetUserDTO GetUserService { get; set; }
        private IUOWFactory DataFactory { get; set; }

        public AdminService(IGetUserDTO getUserDTO, IUOWFactory dataFactory)
        {
            GetUserService = getUserDTO;
            DataFactory = dataFactory;
        }

        private AdminUserInfoDTO GetDTO(User user)
        {
            return new AdminUserInfoDTO
            {
                authType = user.authType,
                Login = user.Login,
                Avatar = user.Info?.Avatar,
                email = user.Email?.Value,
                phone = user.Phone?.Value,
                emailConfirm = user.Email?.Confirmed,
                phoneConfirm = user.Phone?.Confirmed,
                Id = user.Id,
                Level = user.Level,
                name = user.Info?.FullName,
                Banned = user.Ban != null,
                BanReason = user.Ban?.Reason,
                BanTime = user.Ban?.EndTime
            };
        }

        private void EditUser(User user, AdminUserInfoDTO userInfo)
        {
            if(userInfo.Level >= 0 || userInfo.Level <= 2)
                user.Level = userInfo.Level;
            user.authType = userInfo.authType;
            if (user.Info != null)
            {
                if(userInfo.Avatar != null)
                    user.Info.Avatar = userInfo.Avatar;
                if(userInfo.name != null)
                user.Info.FullName = userInfo.name;
            }
            if (user.Phone != null)
            {
                if(userInfo.phone != null)
                    user.Phone.Value = userInfo.phone;
                user.Phone.Confirmed = userInfo.phoneConfirm != null && userInfo.phoneConfirm == true;
            }
            else if(userInfo.phone != null)
            {
                user.Phone = new Contact
                {
                    Confirmed = userInfo.phoneConfirm != null && userInfo.phoneConfirm == true,
                    Value = userInfo.phone
                };
            }
            if (user.Email != null)
            {
                if(userInfo.email != null)
                    user.Email.Value = userInfo.email;
                user.Email.Confirmed = userInfo.emailConfirm != null && userInfo.emailConfirm == true;
            }
            else if (userInfo.email != null)
            {
                user.Email = new Contact
                {
                    Confirmed = userInfo.emailConfirm != null && userInfo.emailConfirm == true,
                    Value = userInfo.email
                };
            }
        }

        private void EditUserBan(User user, AdminUserInfoDTO userInfo, IUOW Data)
        {
            if (user.Ban == null)
            {
                if (userInfo.Banned)
                    user.Ban = new Ban { Reason = "-", EndTime = DateTime.Now.AddDays(1) };
            }

            if (user.Ban != null)
            {
                if (!userInfo.Banned)
                    Data.BanRepository.Delete(user.Ban);
                if (userInfo.BanReason != null)
                    user.Ban.Reason = userInfo.BanReason;
                if (userInfo.BanTime != null)
                    user.Ban.EndTime = (DateTime)userInfo.BanTime;
            }
        }

        public bool IsAdmin(UserDTO userDTO)
        {
            using(var Data = DataFactory.Get())
            {
                var user = GetUserService.Get(userDTO, Data);
                if (user == null || user.Level < 2) return false;
                else return true;
            }
        }

        public (AccessCode Code,IEnumerable<AdminUserInfoDTO> Result) Search(UserDTO userDTO, string name)
        {
            using(var Data = DataFactory.Get())
            {
                var user = GetUserService.Get(userDTO, Data);
                if (user == null || user.Level < 2) return (AccessCode.NoPremision, null);
                var users = Data.UserRepository.Get().Where(x => x.Login.Contains(name) || x.Info.FullName.Contains(name));
                var res = new List<AdminUserInfoDTO>();
                foreach (var us in users)
                    res.Add(GetDTO(us));
                return (AccessCode.Succsess, res);
            }
        }

        public (AccessCode Code, IEnumerable<AdminUserInfoDTO> Result) GetAll(UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var user = GetUserService.Get(userDTO, Data);
                if (user == null || user.Level < 2) return (AccessCode.NoPremision, null);
                var users = Data.UserRepository.Get();
                var res = new List<AdminUserInfoDTO>();
                foreach (var us in users)
                    res.Add(GetDTO(us));
                return (AccessCode.Succsess, res);
            }
        }

        public (AccessCode Code, AdminUserInfoDTO Result) GetUser(UserDTO userDTO, int id)
        {
            using (var Data = DataFactory.Get())
            {
                var user = GetUserService.Get(userDTO, Data);
                if (user == null || user.Level < 2) return (AccessCode.NoPremision, null);
                var res = Data.UserRepository.Get().FirstOrDefault(x => x.Id == id);
                if (res == null) return (AccessCode.NotFound, null);
                return (AccessCode.Succsess, GetDTO(res)); 
                
            }
        }

        public AccessCode EditUser(UserDTO userDTO, AdminUserInfoDTO userInfo)
        {
            using (var Data = DataFactory.Get())
            {
                var admin = GetUserService.Get(userDTO, Data);
                if (admin == null || admin.Level < 2) return AccessCode.NoPremision;
                var user = Data.UserRepository.Get().FirstOrDefault(x => x.Id == userInfo.Id);
                if (user == null) return AccessCode.NotFound;
                EditUser(user, userInfo);
                EditUserBan(user, userInfo, Data);
                Data.UserRepository.Edited(user);
                Data.SaveChanges();
                return AccessCode.Succsess;
            }
        }

        public AccessCode ResetClaims(UserDTO userDTO, int UserId)
        {
            using (var Data = DataFactory.Get())
            {
                var admin = GetUserService.Get(userDTO, Data);
                if (admin == null || admin.Level < 2) return AccessCode.NoPremision;
                var user = Data.UserRepository.Get().FirstOrDefault(x => x.Id == UserId);
                if (user == null) return AccessCode.NotFound;
                var claims = Data.UserClaimRepository.Get().Where(x => x.User == user);
                Data.UserClaimRepository.Delete(claims);
                Data.SaveChanges();
                return AccessCode.Succsess;
            }
        }

        

    }
}
