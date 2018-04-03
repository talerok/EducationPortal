using System;
using System.Linq;
using System.Text.RegularExpressions;
using Education.BLL.DTO.User;
using Education.BLL.Services.UserServices.Interfaces;
using Education.DAL.Interfaces;
using Education.BLL.Logic;

namespace Education.BLL.Services.UserServices.Auth
{
    public class RegValidator : IRegValidator
    {
        private Regex PhoneRegex = new Regex(regRegularExpressions.PhoneRegex);
        private Regex EmailRegex = new Regex(regRegularExpressions.EmailRegex);
        private Regex LoginRegex = new Regex(regRegularExpressions.LoginRegex);
        private Regex PassRegex = new Regex(regRegularExpressions.PasswordRegex);
        private Regex FullNameRegex = new Regex(regRegularExpressions.FullNameRegex);

        private IUOW Data;

        public RegValidator(IUOW uow)
        {
            Data = uow;
        }

        public CheckResult checkEmail(string email)
        {
            if (email == null || !EmailRegex.IsMatch(email)) return CheckResult.WrongValue;
            if (Data.UserRepository.Get().FirstOrDefault(x => x.Email.Value == email) != null)
                return CheckResult.AlreadyExists;
            return CheckResult.Ok;
        }

        public CheckResult checkPhone(string phone)
        {
            if (phone == null || !PhoneRegex.IsMatch(phone)) return CheckResult.WrongValue;
            if (Data.UserRepository.Get()
                .FirstOrDefault(x => x.Phone.Value == phone) != null)
                return CheckResult.AlreadyExists;
            return CheckResult.Ok;
        }

        public CheckResult checkLogin(string login)
        {
            if (login == null || !LoginRegex.IsMatch(login))
                return CheckResult.WrongValue;
            if (Data.UserRepository.Get().FirstOrDefault(x => x.Login == login) != null)
                return CheckResult.AlreadyExists;
            return CheckResult.Ok;
        }

        public CheckResult checkPassword(string password)
        {
            if (password == null || !PassRegex.IsMatch(password))
                return CheckResult.WrongValue;
            return CheckResult.Ok;
        }

        public CheckResult checkFullName(string fullname)
        {
            if (fullname == null || !FullNameRegex.IsMatch(fullname))
                return CheckResult.WrongValue;
            return CheckResult.Ok;
        }

        public RegisterResult Check(UserDTO userDTO)
        {
            bool emailExists = !String.IsNullOrEmpty(userDTO.Email);
            bool phoneExists = !String.IsNullOrEmpty(userDTO.PhoneNumber);

            if (!emailExists && !phoneExists) return RegisterResult.NeedContact;

            var Check = checkLogin(userDTO.Login);
            if (Check == CheckResult.AlreadyExists) return RegisterResult.LoginAlreadyExists;
            else if (Check == CheckResult.WrongValue) return RegisterResult.WrongLogin;

            Check = checkPassword(userDTO.Password);
            if (Check == CheckResult.WrongValue) return RegisterResult.WrongPassword;

            if (phoneExists)
            {
                Check = checkPhone(userDTO.PhoneNumber);
                if (Check == CheckResult.AlreadyExists) return RegisterResult.PhoneAlreadyExists;
                else if (Check == CheckResult.WrongValue) return RegisterResult.WrongPhone;
            }

            if (emailExists)
            {
                Check = checkEmail(userDTO.Email);
                if (Check == CheckResult.AlreadyExists) return RegisterResult.EmailAlreadyExists;
                else if (Check == CheckResult.WrongValue) return RegisterResult.WrongEmail;
            }

            if (checkFullName(userDTO.FullName) != CheckResult.Ok) return RegisterResult.WrongFullName;

            return RegisterResult.Confirm;           

        }
    }
}