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

        public RegValidator()
        {

        }

        public CheckResult checkEmail(string email, IUOW Data)
        {
            if (email == null || !EmailRegex.IsMatch(email)) return CheckResult.WrongValue;
            if (Data.UserRepository.Get().FirstOrDefault(x => x.Email.Value == email) != null)
                return CheckResult.AlreadyExists;
            return CheckResult.Ok;
        }

        public CheckResult checkPhone(string phone, IUOW Data)
        {
            if (phone == null || !PhoneRegex.IsMatch(phone)) return CheckResult.WrongValue;
            if (Data.UserRepository.Get()
                .FirstOrDefault(x => x.Phone.Value == phone) != null)
                return CheckResult.AlreadyExists;
            return CheckResult.Ok;
        }

        public CheckResult checkLogin(string login, IUOW Data)
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

        public RegisterResult Check(RegUserInfo userDTO, IUOW Data)
        {
            bool emailExists = !String.IsNullOrEmpty(userDTO.Email);
            bool phoneExists = !String.IsNullOrEmpty(userDTO.Phone);

            if (!emailExists && !phoneExists) return RegisterResult.NeedContact;

            var Check = checkLogin(userDTO.Login, Data);
            if (Check == CheckResult.AlreadyExists) return RegisterResult.LoginAlreadyExists;
            else if (Check == CheckResult.WrongValue) return RegisterResult.WrongLogin;

            Check = checkPassword(userDTO.Password);
            if (Check == CheckResult.WrongValue) return RegisterResult.WrongPassword;

            if (phoneExists)
            {
                Check = checkPhone(userDTO.Phone, Data);
                if (Check == CheckResult.AlreadyExists) return RegisterResult.PhoneAlreadyExists;
                else if (Check == CheckResult.WrongValue) return RegisterResult.WrongPhone;
            }

            if (emailExists)
            {
                Check = checkEmail(userDTO.Email, Data);
                if (Check == CheckResult.AlreadyExists) return RegisterResult.EmailAlreadyExists;
                else if (Check == CheckResult.WrongValue) return RegisterResult.WrongEmail;
            }

            if (checkFullName(userDTO.Name) != CheckResult.Ok) return RegisterResult.WrongFullName;

            return RegisterResult.Confirm;           

        }
    }
}