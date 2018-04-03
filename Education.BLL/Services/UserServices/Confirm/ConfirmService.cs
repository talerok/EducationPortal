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
        protected IUOW Data;

        public ConfirmService(IAuthKeyService keyService, IUOW data)
        {
            Data = data;
            KeyService = keyService;
        }

        private void RemoveContact(Contact contact)
        {
            Data.ContactRepository.Delete(contact);
        }

        private void ConfirmContact(Contact contact)
        {
            contact.Confirmed = true;
            Data.ContactRepository.Edited(contact);
        }

        public ConfirmResult DoIfSuccsess(Contact contact, Action<Contact> action = null, string key = null)
        {
            if (contact == null) return new ConfirmResult { Status = ConfirmCode.ContactNotFound };
            if (String.IsNullOrEmpty(key))
            {
                var keytime = KeyService.Generate(contact);
                return new ConfirmResult { Status = ConfirmCode.KeySend, KeyTime = keytime };
            }
            var res = KeyService.Check(contact, key);
            if (res == KeyStatus.Success)
            {
                action?.Invoke(contact);
                return new ConfirmResult { Status = ConfirmCode.Success };
            }
            else if (res == KeyStatus.KeyTimeEnded) return new ConfirmResult { Status = ConfirmCode.NeedNewKey };
            return new ConfirmResult { Status = ConfirmCode.Fail };
        }

        public virtual ConfirmResult Confirm(Contact contact, string key = null)
        {
            if (contact == null) return new ConfirmResult { Status = ConfirmCode.ContactNotFound };
            if (contact.Confirmed) return new ConfirmResult { Status = ConfirmCode.AlreadyConfimed };
            return DoIfSuccsess(contact, ConfirmContact, key);
        }

        public virtual ConfirmResult Remove(Contact contact, string key = null)
        {
            if (contact == null) return new ConfirmResult { Status = ConfirmCode.ContactNotFound };
            if (!contact.Confirmed)
            {
                RemoveContact(contact);
                return new ConfirmResult { Status = ConfirmCode.Success };
            }
            return DoIfSuccsess(contact, RemoveContact, key);
        }
    }
}