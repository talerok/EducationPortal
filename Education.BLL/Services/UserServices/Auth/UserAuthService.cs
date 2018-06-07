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
using Education.BLL.Logic.Interfaces;

namespace Education.BLL.Services.UserServices.Auth
{
    public class UserAuthService : IUserService
    {
        private IUOWFactory DataFactory;
        private IAuthKeyService EmailAuthService;
        private IAuthKeyService PhoneAuthService;
        private IPassHasher PassHasher;
        private IRegValidator RegValidator;
        private IKeyGenerator KeyGenerator;
        private IGetUserDTO GetUserService;
        public IClaimService ClaimService { get; private set; }

        public UserAuthService(IUOWFactory iuowf, 
            IAuthKeyService phoneAuthService, 
            IAuthKeyService emailAuthService, 
            IPassHasher passHasher,
            IRegValidator regValidator,
            IClaimService claimService,
            IKeyGenerator keyGenerator,
            IGetUserDTO getUserDTO)
        {
            DataFactory = iuowf;
            EmailAuthService = emailAuthService;
            PhoneAuthService = phoneAuthService;
            PassHasher = passHasher;
            RegValidator = regValidator;
            ClaimService = claimService;
            KeyGenerator = keyGenerator;
            GetUserService = getUserDTO;

        }
      
        private User GetUser(string login, string pass, IUOW Data)
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

        

        private AuthResult KeyLogin(User user, LoginInfoDTO loginInfoDTO, IUOW Data)
        {
            KeyStatus keyStatus;
            if (user.authType == AuthType.Email) keyStatus = EmailAuthService.Check(user.Email, loginInfoDTO.Key, Data);
            else if (user.authType == AuthType.Phone) keyStatus = PhoneAuthService.Check(user.Phone, loginInfoDTO.Key, Data);
            else throw new UserAuthException(AuthError.AuthTypeNotFound);
            if (keyStatus == KeyStatus.Success)
                return new AuthResult { Status = AuthStatus.Succsess, Identity = ClaimService.Generate(user, Data, loginInfoDTO) };
            else if (keyStatus == KeyStatus.KeyTimeEnded) return new AuthResult { Status = AuthStatus.NeedNewKey };
            return new AuthResult { Status = AuthStatus.WrongKey };

        }

        private AuthResult SendKey(User user, IUOW Data)
        {
            DateTime keyTime;
            if (user.authType == AuthType.Email) keyTime = EmailAuthService.Generate(user.Email, Data);
            else if (user.authType == AuthType.Phone) keyTime = PhoneAuthService.Generate(user.Phone, Data);
            else throw new UserAuthException(AuthError.AuthTypeNotFound);
            return new AuthResult { Status = AuthStatus.KeySent, authType = user.authType, KeyTime = keyTime };
        }


        public AuthResult Login(LoginInfoDTO loginInfoDTO)
        {
            using (var Data = DataFactory.Get())
            {
                User user = GetUser(loginInfoDTO.Login, loginInfoDTO.Password, Data);
                if (user == null) return new AuthResult { Status = AuthStatus.UserNotFound };
                if (user.Ban != null)
                {
                    if (user.Ban.EndTime < DateTime.Now) return new AuthResult { Status = AuthStatus.UserBanned, KeyTime = user.Ban.EndTime, Comment = user.Ban.Reason };
                    else
                    {
                        Data.BanRepository.Delete(user.Ban);
                        Data.SaveChanges();
                    }
                }
                if (user.authType == AuthType.Simple)
                    return new AuthResult { Status = AuthStatus.Succsess, Identity = ClaimService.Generate(user, Data, loginInfoDTO) };
                else if (!String.IsNullOrEmpty(loginInfoDTO.Key)) return KeyLogin(user, loginInfoDTO, Data);
                else return SendKey(user, Data);
            }
        }

        public void Logout(IEnumerable<Claim> claims)
        {
            ClaimService.Logout(claims);
        }
                     
        public RegisterResult Register(RegUserInfo userInfo)
        {
            using (var Data = DataFactory.Get())
            {
                var info = new UserInfo { FullName = userInfo.Name };
                var check = RegValidator.Check(userInfo, Data);
                if (check != RegisterResult.Confirm) return check;
                var newUser = new User
                {
                    Info = info,
                    Login = userInfo.Login.ToLower(),
                    Password = PassHasher.Get(userInfo.Password)
                };

                Contact email = null;
                Contact phone = null;

                if (!String.IsNullOrEmpty(userInfo.Email)) {
                    email = new Contact { Value = userInfo.Email.ToLower(), Confirmed = false };
                }
                if (!String.IsNullOrEmpty(userInfo.Phone))
                {
                    phone = new Contact { Value = userInfo.Phone, Confirmed = false };
                }

                newUser.Phone = phone;
                newUser.Email = email;
                Data.UserRepository.Add(newUser);
                Data.SaveChanges();
            }
            return RegisterResult.Confirm;
        }

        public void ResetClaims(UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                ClaimService.RemoveAllClaims(GetUserService.Get(userDTO,Data),Data);
            }
        }

    }
}
