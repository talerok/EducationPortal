using Education.BLL.DTO;
using Education.BLL.Services.UserServices.Interfaces;
using Education.DAL.Entities;
using Education.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Education.BLL.Services.UserServices.Confirm
{
    public class ConfirmService : IConfirmService
    {
        protected IAuthKeyService KeyService;

        public ConfirmService(IAuthKeyService keyService)
        {
            KeyService = keyService;
        }

        private void RemoveContact(Contact contact, IUOW Data)
        {
            Data.ContactRepository.Delete(contact);
            Data.SaveChanges();
        }

        private void ConfirmContact(Contact contact, IUOW Data)
        {
            contact.Confirmed = true;
            Data.ContactRepository.Edited(contact);
            Data.SaveChanges();
        }

        public ConfirmResult DoIfSuccsess(Contact contact, IUOW Data, Action<Contact, IUOW> action = null, string key = null)
        {
            if (contact == null) return new ConfirmResult { Status = ConfirmCode.ContactNotFound };
            if (String.IsNullOrEmpty(key))
            {
                var keytime = KeyService.Generate(contact, Data);
                return new ConfirmResult { Status = ConfirmCode.KeySend, KeyTime = keytime };
            }
            var res = KeyService.Check(contact, key, Data);
            if (res == KeyStatus.Success)
            {
                action?.Invoke(contact, Data);
                return new ConfirmResult { Status = ConfirmCode.Success };
            }
            else if (res == KeyStatus.KeyTimeEnded) return new ConfirmResult { Status = ConfirmCode.NeedNewKey };
            return new ConfirmResult { Status = ConfirmCode.Fail };
        }

        public virtual ConfirmResult Confirm(Contact contact, IUOW Data, string key = null)
        {
            if (contact == null) return new ConfirmResult { Status = ConfirmCode.ContactNotFound };
            if (contact.Confirmed) return new ConfirmResult { Status = ConfirmCode.AlreadyConfimed };
            return DoIfSuccsess(contact, Data, ConfirmContact, key);
        }

        public virtual ConfirmResult Remove(Contact contact, IUOW Data, string key = null)
        {
            if (contact == null) return new ConfirmResult { Status = ConfirmCode.ContactNotFound };
            if (!contact.Confirmed)
            {
                RemoveContact(contact, Data);
                return new ConfirmResult { Status = ConfirmCode.Success };
            }
            return DoIfSuccsess(contact, Data, RemoveContact, key);
        }
    }
}