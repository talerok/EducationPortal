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

namespace Education.BLL.Services.UserServices.Profile
{
    public class ProfileService : IProfileService
    {
        private IPassHasher PassHasher;

        private static string EmailPropertyName = "Email";
        private static string PhonePropertyName = "Phone";

        private IUOW Data;
        private IRegValidator RegValidator;

        private IConfirmService EmailService;
        private IConfirmService PhoneService;

        public IClaimService ClaimService { get; private set; }

        public ProfileService(
            IUOW uow, 
            IRegValidator regValidator, 
            IConfirmService emailCS, 
            IConfirmService phoneCS, 
            IPassHasher passHasher,
            IClaimService claimService) 
        {
            Data = uow;
            RegValidator = regValidator;
            EmailService = emailCS;
            PhoneService = phoneCS;
            PassHasher = passHasher;
            ClaimService = claimService;
        }

        private User GetUser(UserDTO userDTO)
        {
            if (userDTO == null) return null; 
            var name = userDTO.Login.ToLower();
            return Data.UserRepository.Get().FirstOrDefault(x => x.Login == name
            && x.Password == userDTO.Password);
        }

        private void SetPassword(User user, string password)
        {
            user.Password = PassHasher.Get(password);
            Data.UserRepository.Edited(user);
        }

        #region ContactLogic

        private ConfirmResult ConfirmContact(UserDTO userDTO, string propertyName, Func<Contact, string, ConfirmResult> func, string key = null)
        {
            var User = GetUser(userDTO);
            if (User == null) return new ConfirmResult { Status = ConfirmCode.UserNotFound };
            var Property = User.GetType().GetProperty(propertyName);
            var Contact = Property.GetValue(User) as Contact;
            if (Contact == null) return new ConfirmResult { Status = ConfirmCode.ContactNotFound };
            return func(Contact, key);
        }

        private ConfirmResult RemoveContact(User User, string propertyName, Func<Contact, string, ConfirmResult> func, AuthType checkType, string key = null)
        {
            if (User == null) return new ConfirmResult { Status = ConfirmCode.UserNotFound };
            var Property = User.GetType().GetProperty(propertyName);
            var Contact = Property.GetValue(User) as Contact;
            if (Contact == null) return new ConfirmResult { Status = ConfirmCode.ContactNotFound };
            var res = func(Contact, key);
            if (res.Status == ConfirmCode.Success && User.authType == checkType)
            {
                Property.SetValue(User, null);
                User.authType = AuthType.Simple;
                Data.UserRepository.Edited(User);
            }
            return res;
        }

        private SetContactCode SetContact(UserDTO userDTO, string value, string propertyName, Func<string, CheckResult> checkFunc)
        {
            var User = GetUser(userDTO);
            if (User == null) return SetContactCode.UserNotFound;
            if (User.GetType().GetProperty(propertyName).GetValue(User) != null) return SetContactCode.AlreadySet;
            var check = checkFunc(value);
            if (check == CheckResult.AlreadyExists)
                return SetContactCode.AlreadyExists;
            else if (check == CheckResult.WrongValue)
                return SetContactCode.WrongValue;

            User.GetType().GetProperty(propertyName).SetValue(User, new Contact { Confirmed = false, Value = value });
            Data.UserRepository.Edited(User);
            return SetContactCode.Succsess;
        }   

        #endregion

        #region ContactServices

        public ConfirmResult ConfirmEmail(UserDTO userDTO, string key = null)
        {
            return ConfirmContact(userDTO, EmailPropertyName, EmailService.Confirm, key);
        }

        public ConfirmResult ConfirmPhone(UserDTO userDTO, string key = null)
        {
            return ConfirmContact(userDTO, PhonePropertyName, PhoneService.Confirm, key);
        }

        public ConfirmResult RemoveEmail(UserDTO userDTO, string key = null)
        {
            var User = GetUser(userDTO);
            if (User == null) return new ConfirmResult { Status = ConfirmCode.UserNotFound };
            return RemoveContact(User, EmailPropertyName, EmailService.Remove, AuthType.Email, key);
        }

        public ConfirmResult RemovePhone(UserDTO userDTO, string key = null)
        {
            var User = GetUser(userDTO);
            if (User == null) return new ConfirmResult { Status = ConfirmCode.UserNotFound };
            return RemoveContact(User, PhonePropertyName, PhoneService.Remove, AuthType.Phone, key);
        }

        public SetContactCode SetEmail(UserDTO userDTO, string email)
        {
            return SetContact(userDTO, email, EmailPropertyName, RegValidator.checkEmail);
        }

        public SetContactCode SetPhone(UserDTO userDTO, string phone)
        {
            return SetContact(userDTO, phone, PhonePropertyName, RegValidator.checkPhone);
        }

        #endregion

        public UserProfileDTO GetUserProfile(UserDTO userDTO)
        {
            var user = GetUser(userDTO);
            if (user == null) return null;
            return new UserProfileDTO
            {
                authType = user.authType,
                email = user.Email?.Value,
                emailConfirm = user.Email?.Confirmed,
                phone = user.Phone?.Value,
                phoneConfirm = user.Phone?.Confirmed,
                Claims = ClaimService.GetInfo(user)
        };
        }

        public ConfirmResult ResetAuthType(UserDTO userDTO, string key = null)
        {
            var User = GetUser(userDTO);
            ConfirmResult res;
            if (User == null) return new ConfirmResult { Status = ConfirmCode.UserNotFound };
            if (User.authType == AuthType.Simple) return new ConfirmResult { Status = ConfirmCode.Success };

            else if (User.authType == AuthType.Email) res = EmailService.DoIfSuccsess(User.Email, null, key);
            else if (User.authType == AuthType.Phone) res = PhoneService.DoIfSuccsess(User.Phone, null, key);
            else return new ConfirmResult { Status = ConfirmCode.ContactNotFound };

            if (res.Status == ConfirmCode.Success)
            {
                User.authType = AuthType.Simple;
                Data.UserRepository.Edited(User);
                return new ConfirmResult { Status = ConfirmCode.Success };
            }
            return res;
        }

        public ConfirmResult SetAuthType(UserDTO userDTO, AuthType authType, string key = null)
        {
            ConfirmResult res;
            var User = GetUser(userDTO);
            if (User == null) return new ConfirmResult { Status = ConfirmCode.UserNotFound };
            if (User.authType != AuthType.Simple) return new ConfirmResult { Status = ConfirmCode.NeedContact };
            if (authType == AuthType.Simple) return new ConfirmResult { Status = ConfirmCode.Success };
            else if (authType == AuthType.Phone && User.Phone != null && User.Phone.Confirmed)
            {
                res = PhoneService.DoIfSuccsess(User.Phone, null, key);
            }
            else if (authType == AuthType.Email && User.Email != null && User.Email.Confirmed)
            {
                res = EmailService.DoIfSuccsess(User.Email, null, key);
            }
            else return new ConfirmResult { Status = ConfirmCode.ContactNotFound };

            if(res.Status == ConfirmCode.Success)
            {
                User.authType = authType;
                Data.UserRepository.Edited(User);
            }
            return res;
        }

        public ConfirmResult SetPassword(UserDTO userDTO, string oldpassword, string newPassword, string key = null)
        {
            var User = GetUser(userDTO);
            ConfirmResult res;
            if (User == null || PassHasher.Get(oldpassword) != User.Password) return new ConfirmResult { Status = ConfirmCode.UserNotFound };
            if (User.authType == AuthType.Simple)
            {
                SetPassword(User, newPassword);
                return new ConfirmResult { Status = ConfirmCode.Success };
            }
           
            else if (User.authType == AuthType.Email) res = EmailService.DoIfSuccsess(User.Email, null, key);
            else if (User.authType == AuthType.Phone) res = PhoneService.DoIfSuccsess(User.Phone, null, key);
            else return new ConfirmResult { Status = ConfirmCode.ContactNotFound };

            if(res.Status == ConfirmCode.Success)
            {
                SetPassword(User, newPassword);
                return new ConfirmResult { Status = ConfirmCode.Success };
            }
            return res;
        }

    }
}