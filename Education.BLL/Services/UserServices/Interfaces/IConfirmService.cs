using Education.BLL.DTO;
using Education.DAL.Entities;
using Education.DAL.Interfaces;
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
        ConfirmResult Confirm(Contact contact, IUOW Data, string key = null);
        ConfirmResult Remove(Contact contact, IUOW Data, string key = null);
        ConfirmResult DoIfSuccsess(Contact contact, IUOW Data, Action<Contact, IUOW> action = null, string key = null);
    }
}
