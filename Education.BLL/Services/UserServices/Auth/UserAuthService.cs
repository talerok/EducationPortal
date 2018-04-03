using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using Education.BLL.DTO.User;
using Education.BLL.Logic;
using Education.BLL.DTO;
using Education.DAL.Entities;
using Education.DAL.Interfaces;
using Education.BLL.Services.UserServices.Interfaces;

namespace Education.BLL.Services.UserServices.Auth
{
    public class UserAuthService : Interfaces.IUserService
    {
        private IUOW Data;
        private IAuthKeyService EmailAuthService;
        private IAuthKeyService PhoneAuthService;
        private IPassHasher PassHasher;
        private IRegValidator RegValidator;
        private IKeyGenerator KeyGenerator;
        public IClaimService ClaimService { get; private set; }

        public UserAuthService(IUOW iuow, 
            IAuthKeyService phoneAuthService, 
            IAuthKeyService emailAuthService, 
            IPassHasher passHasher,
            IRegValidator regValidator,
            IClaimService claimService,
            IKeyGenerator keyGenerator)
        {
            Data = iuow;
            EmailAuthService = emailAuthService;
            PhoneAuthService = phoneAuthService;
            PassHasher = passHasher;
            RegValidator = regValidator;
            ClaimService = claimService;
            KeyGenerator = keyGenerator;

        }
      
        private User GetUser(string login, string pass)
        {
            return Data.UserRepository.Get().FirstOrDefault(
                x => 
                x.Password == PassHasher.Get(pass) 
                && 
                x.Login == login.ToLower()
                || 
                x.Phone != null && x.Phone.Confirmed && x.Phone.Value == login.ToLower()
                || 
                x.Email != null && x.Email.Confirmed && x.Email.Value == login.ToLower());
        }

        private User GetUser(UserDTO userDTO)
        {
            return Data.UserRepository.Get().FirstOrDefault(
                x =>
                x.Password == userDTO.Password
                &&
                x.Login == userDTO.Login.ToLower()
                ||
                x.Phone != null && x.Phone.Confirmed && x.Phone.Value == userDTO.PhoneNumber.ToLower()
                ||
                x.Email != null && x.Email.Confirmed && x.Email.Value == userDTO.Email.ToLower());
        }

        private AuthResult KeyLogin(User user, LoginInfoDTO loginInfoDTO)
        {
            KeyStatus keyStatus;
            if (user.authType == AuthType.Email) keyStatus = EmailAuthService.Check(user.Email, loginInfoDTO.Key);
            else if (user.authType == AuthType.Phone) keyStatus = PhoneAuthService.Check(user.Phone, loginInfoDTO.Key);
            else throw new UserAuthException(AuthError.AuthTypeNotFound);
            if (keyStatus == KeyStatus.Success)
                return new AuthResult { Status = AuthStatus.Succsess, Identity = ClaimService.Generate(user, loginInfoDTO) };
            else if (keyStatus == KeyStatus.KeyTimeEnded) return new AuthResult { Status = AuthStatus.NeedNewKey };
            return new AuthResult { Status = AuthStatus.WrongKey };

        }

        private AuthResult SendKey(User user)
        {
            DateTime keyTime;
            if (user.authType == AuthType.Email) keyTime = EmailAuthService.Generate(user.Email);
            else if (user.authType == AuthType.Phone) keyTime = PhoneAuthService.Generate(user.Phone);
            else throw new UserAuthException(AuthError.AuthTypeNotFound);
            return new AuthResult { Status = AuthStatus.KeySent, authType = user.authType, KeyTime = keyTime };
        }

        public AuthResult Login(LoginInfoDTO loginInfoDTO)
        {
            User user = GetUser(loginInfoDTO.Login, loginInfoDTO.Password);
            if (user == null) return new AuthResult { Status = AuthStatus.UserNotFound };
            if (user.Ban != null)
            {
                if (user.Ban.EndTime < DateTime.Now) return new AuthResult { Status = AuthStatus.UserBanned, KeyTime = user.Ban.EndTime, Comment = user.Ban.Reason };
                else Data.BanRepository.Delete(user.Ban);
            }
            if (user.authType == AuthType.Simple)
                return new AuthResult { Status = AuthStatus.Succsess, Identity = ClaimService.Generate(user,loginInfoDTO) };
            else if (!String.IsNullOrEmpty(loginInfoDTO.Key)) return KeyLogin(user, loginInfoDTO);
            else return SendKey(user);
        }

        public void Logout(IEnumerable<Claim> claims)
        {
            ClaimService.Logout(claims);
        }
                     
        public RegisterResult Register(UserDTO userDTO)
        {        
            var info = new UserInfo { FullName = userDTO.FullName };
            var check = RegValidator.Check(userDTO);
            if (check != RegisterResult.Confirm) return check;
            var newUser = new User
            {
                Info = info,
                Login = userDTO.Login.ToLower(),
                Password = PassHasher.Get(userDTO.Password)
            };

            Contact email = null;
            Contact phone = null;

            if (!String.IsNullOrEmpty(userDTO.Email)) {
                email = new Contact { Value = userDTO.Email.ToLower(), Confirmed = false };
            }
            if (!String.IsNullOrEmpty(userDTO.PhoneNumber))
            {
                phone = new Contact { Value = userDTO.PhoneNumber, Confirmed = false };
            }

            newUser.Phone = phone;
            newUser.Email = email;
            Data.UserRepository.Add(newUser);
            return RegisterResult.Confirm;
        }

        public void ResetClaims(UserDTO userDTO)
        {
            var user = GetUser(userDTO);
            ClaimService.RemoveAllClaims(user);
        }

    }
}
