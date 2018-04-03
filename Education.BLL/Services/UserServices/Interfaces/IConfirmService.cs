using Education.BLL.DTO;
using Education.DAL.Entities;
using System;

namespace Education.BLL.Services.UserServices.Interfaces
{
    public enum ConfirmCode
    {
        Success,
        KeySend,
        Fail,
        NeedNewKey,
        UserNotFound,
        AlreadyConfimed,
        ContactNotFound,
        NeedContact
    };

    public interface IConfirmService
    {
        ConfirmResult Confirm(Contact contact, string key = null);
        ConfirmResult Remove(Contact contact, string key = null);
        ConfirmResult DoIfSuccsess(Contact contact, Action<Contact> action = null, string key = null);
    }
}
