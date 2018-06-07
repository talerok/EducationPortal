using Education.BLL.DTO.User;
using Education.BLL.DTO;
using Education.BLL.Services.UserServices.Interfaces;
using Education.DAL.Entities;
using Education.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Education.BLL.Services.ImageManager.Interfaces;
using Education.BLL.Logic.Interfaces;

namespace Education.BLL.Services.UserServices.Profile
{
    public class ProfileService : IProfileService
    {
        private IPassHasher PassHasher;

        private static string EmailPropertyName = "Email";
        private static string PhonePropertyName = "Phone";

        private IUOWFactory DataFactory;
        private IRegValidator RegValidator;

        private IConfirmService EmailService;
        private IConfirmService PhoneService;
        private IGetUserDTO GetUserService;

        public IClaimService ClaimService { get; private set; }

        public ProfileService(
            IUOWFactory uowf, 
            IRegValidator regValidator, 
            IConfirmService emailCS, 
            IConfirmService phoneCS, 
            IPassHasher passHasher,
            IClaimService claimService,
            IGetUserDTO getUserDTO) 
        {
            DataFactory = uowf;
            RegValidator = regValidator;
            EmailService = emailCS;
            PhoneService = phoneCS;
            PassHasher = passHasher;
            ClaimService = claimService;
            GetUserService = getUserDTO;
        }

        private void SetPassword(User user, string password, IUOW Data)
        {
            user.Password = PassHasher.Get(password);
            Data.UserRepository.Edited(user);
            Data.SaveChanges();
        }

        #region ContactLogic

        private ConfirmResult ConfirmContact(UserDTO userDTO, string propertyName, Func<Contact, IUOW, string, ConfirmResult> func, IUOW Data, string key = null)
        {
            var User = GetUserService.Get(userDTO, Data);
            if (User == null) return new ConfirmResult { Status = ConfirmCode.UserNotFound };
            var Property = User.GetType().GetProperty(propertyName);
            var Contact = Property.GetValue(User) as Contact;
            if (Contact == null) return new ConfirmResult { Status = ConfirmCode.ContactNotFound };
            return func(Contact, Data, key);
        }

        private ConfirmResult RemoveContact(User User, string propertyName, Func<Contact, IUOW, string, ConfirmResult> func, AuthType checkType, IUOW Data, string key = null)
        {
            if (User == null) return new ConfirmResult { Status = ConfirmCode.UserNotFound };
            var Property = User.GetType().GetProperty(propertyName);
            var Contact = Property.GetValue(User) as Contact;
            if (Contact == null) return new ConfirmResult { Status = ConfirmCode.ContactNotFound };
            var res = func(Contact, Data, key);
            if (res.Status == ConfirmCode.Success && User.authType == checkType)
            {
                Property.SetValue(User, null);
                User.authType = AuthType.Simple;
                Data.UserRepository.Edited(User);
                Data.SaveChanges();
            }
            return res;
        }

        private SetContactCode SetContact(UserDTO userDTO, string value, string propertyName, Func<string, IUOW, CheckResult> checkFunc, IUOW Data)
        {
            var User = GetUserService.Get(userDTO, Data);
            if (User == null) return SetContactCode.UserNotFound;
            if (User.GetType().GetProperty(propertyName).GetValue(User) != null) return SetContactCode.AlreadySet;
            var check = checkFunc(value, Data);
            if (check == CheckResult.AlreadyExists)
                return SetContactCode.AlreadyExists;
            else if (check == CheckResult.WrongValue)
                return SetContactCode.WrongValue;

            User.GetType().GetProperty(propertyName).SetValue(User, new Contact { Confirmed = false, Value = value });
            Data.UserRepository.Edited(User);
            Data.SaveChanges();
            return SetContactCode.Succsess;
        }   

        #endregion

        #region ContactServices

        public ConfirmResult ConfirmEmail(UserDTO userDTO, string key = null)
        {
            using (var Data = DataFactory.Get())
            {
                return ConfirmContact(userDTO, EmailPropertyName, EmailService.Confirm, Data, key);
            }
        }

        public ConfirmResult ConfirmPhone(UserDTO userDTO, string key = null)
        {
            using (var Data = DataFactory.Get())
            {
                return ConfirmContact(userDTO, PhonePropertyName, PhoneService.Confirm, Data, key);
            }
        }

        public ConfirmResult RemoveEmail(UserDTO userDTO, string key = null)
        {
            using (var Data = DataFactory.Get())
            {
                var User = GetUserService.Get(userDTO, Data);
                if (User == null) return new ConfirmResult { Status = ConfirmCode.UserNotFound };
                return RemoveContact(User, EmailPropertyName, EmailService.Remove, AuthType.Email, Data, key);
            }
        }

        public ConfirmResult RemovePhone(UserDTO userDTO, string key = null)
        {
            using (var Data = DataFactory.Get())
            {
                var User = GetUserService.Get(userDTO, Data);
                if (User == null) return new ConfirmResult { Status = ConfirmCode.UserNotFound };
                return RemoveContact(User, PhonePropertyName, PhoneService.Remove, AuthType.Phone, Data, key);
            }
        }

        public SetContactCode SetEmail(UserDTO userDTO, string email)
        {
            using (var Data = DataFactory.Get())
            {
                return SetContact(userDTO, email, EmailPropertyName, RegValidator.checkEmail,Data);
            }
        }

        public SetContactCode SetPhone(UserDTO userDTO, string phone)
        {
            using (var Data = DataFactory.Get())
            {
                return SetContact(userDTO, phone, PhonePropertyName, RegValidator.checkPhone, Data);
            }
        }

        #endregion

        public UserProfileDTO GetUserProfile(UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var user = GetUserService.Get(userDTO, Data);
                if (user == null) return null;
                return new UserProfileDTO
                {
                    Avatar = user?.Info?.Avatar,
                    authType = user.authType,
                    email = user.Email?.Value,
                    emailConfirm = user.Email?.Confirmed,
                    phone = user.Phone?.Value,
                    phoneConfirm = user.Phone?.Confirmed,
                    name = user.Info?.FullName,
                    Claims = ClaimService.GetInfo(user, Data)
                };
            }
        }

        public ConfirmResult ResetAuthType(UserDTO userDTO, string key = null)
        {
            using (var Data = DataFactory.Get())
            {
                var User = GetUserService.Get(userDTO, Data);
                ConfirmResult res;
                if (User == null) return new ConfirmResult { Status = ConfirmCode.UserNotFound };
                if (User.authType == AuthType.Simple) return new ConfirmResult { Status = ConfirmCode.Success };

                else if (User.authType == AuthType.Email) res = EmailService.DoIfSuccsess(User.Email, Data, null, key);
                else if (User.authType == AuthType.Phone) res = PhoneService.DoIfSuccsess(User.Phone, Data, null, key);
                else return new ConfirmResult { Status = ConfirmCode.ContactNotFound };

                if (res.Status == ConfirmCode.Success)
                {
                    User.authType = AuthType.Simple;
                    Data.UserRepository.Edited(User);
                    Data.SaveChanges();
                    return new ConfirmResult { Status = ConfirmCode.Success };
                }
                return res;
            }
        }

        public ConfirmResult SetAuthType(UserDTO userDTO, AuthType authType, string key = null)
        {
            using (var Data = DataFactory.Get())
            {
                ConfirmResult res;
                var User = GetUserService.Get(userDTO, Data);
                if (User == null) return new ConfirmResult { Status = ConfirmCode.UserNotFound };
                if (User.authType != AuthType.Simple) return new ConfirmResult { Status = ConfirmCode.NeedContact };
                if (authType == AuthType.Simple) return new ConfirmResult { Status = ConfirmCode.Success };
                else if (authType == AuthType.Phone && User.Phone != null && User.Phone.Confirmed)
                {
                    res = PhoneService.DoIfSuccsess(User.Phone, Data, null, key);
                }
                else if (authType == AuthType.Email && User.Email != null && User.Email.Confirmed)
                {
                    res = EmailService.DoIfSuccsess(User.Email, Data, null, key);
                }
                else return new ConfirmResult { Status = ConfirmCode.ContactNotFound };

                if (res.Status == ConfirmCode.Success)
                {
                    User.authType = authType;
                    Data.UserRepository.Edited(User);
                    Data.SaveChanges();
                }
                return res;
            }
        }

        public ConfirmCode SetAvatar(UserDTO userDTO, string path)
        {
            using (var Data = DataFactory.Get())
            {
                var user = GetUserService.Get(userDTO, Data);
                if (user == null) return ConfirmCode.UserNotFound;
                if (user.Info == null) return ConfirmCode.Fail;
                user.Info.Avatar = path;
                Data.UserInfoRepository.Edited(user.Info);
                Data.SaveChanges();
                return ConfirmCode.Success;
            }
        }

        public ConfirmResult SetPassword(UserDTO userDTO, string oldpassword, string newPassword, string key = null)
        {
            using (var Data = DataFactory.Get())
            {
                var User = GetUserService.Get(userDTO, Data);
                ConfirmResult res;
                if (User == null || PassHasher.Get(oldpassword) != User.Password) return new ConfirmResult { Status = ConfirmCode.UserNotFound };
                if (User.authType == AuthType.Simple)
                {
                    SetPassword(User, newPassword,Data);
                    return new ConfirmResult { Status = ConfirmCode.Success };
                }

                else if (User.authType == AuthType.Email) res = EmailService.DoIfSuccsess(User.Email, Data, null, key);
                else if (User.authType == AuthType.Phone) res = PhoneService.DoIfSuccsess(User.Phone, Data, null, key);
                else return new ConfirmResult { Status = ConfirmCode.ContactNotFound };

                if (res.Status == ConfirmCode.Success)
                {
                    SetPassword(User, newPassword,Data);
                    return new ConfirmResult { Status = ConfirmCode.Success };
                }
                return res;
            }
        }

    }
}