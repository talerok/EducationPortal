using Education.BLL.DTO.User;
using Education.DAL.Interfaces;
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
        CheckResult checkEmail(string email, IUOW Data);
        CheckResult checkPhone(string phone, IUOW Data);
        CheckResult checkLogin(string login, IUOW Data);
        CheckResult checkPassword(string password);
        CheckResult checkFullName(string fullname);
        RegisterResult Check(RegUserInfo userDTO, IUOW Data);
    }
}
