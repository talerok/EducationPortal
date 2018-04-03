using Education.BLL.DTO.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.UserServices.Interfaces
{
    public enum RegisterResult
    {
        Confirm,
        WrongLogin,
        WrongEmail,
        WrongPhone,
        WrongPassword,
        WrongFullName,
        LoginAlreadyExists,
        EmailAlreadyExists,
        PhoneAlreadyExists,
        NeedContact,
        InternalError

    };

    public enum CheckResult
    {
        Ok,
        AlreadyExists,
        WrongValue
    };

    public interface IRegValidator
    {
        CheckResult checkEmail(string email);
        CheckResult checkPhone(string phone);
        CheckResult checkLogin(string login);
        CheckResult checkPassword(string password);
        CheckResult checkFullName(string fullname);
        RegisterResult Check(UserDTO userDTO);
    }
}
