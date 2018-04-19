using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Education.BLL.DTO.User;
using Education.BLL.Services.UserServices.Interfaces;
using Education.DAL.Entities;
using Education.BLL.Logic;
using Education.DAL.Interfaces;

namespace Education.BLL.Services.UserServices.Confirm 
{
    public class ConfirmKeyService : IAuthKeyService
    {
        private IMessenger Messager;
        private IKeyGenerator KeyGenerator;

        public ConfirmKeyService(IKeyGenerator kg, IMessenger msgr)
        {
            Messager = msgr;
            KeyGenerator = kg;
        }

        private void RemoveKey(Contact contact, IUOW Data)
        {
            Data.AuthKeyRepository.Delete(contact.ConfirmKey);
            Data.SaveChanges();
        }

        public KeyStatus Check(Contact contact, string Key, IUOW Data)
        {
            if (contact == null) throw new KeyException(KeyError.ContactNotFound);
            if (contact.ConfirmKey == null) return KeyStatus.Fail;
            if (contact.ConfirmKey.EndTime < DateTime.Now)
            {
                RemoveKey(contact, Data);
                return KeyStatus.KeyTimeEnded;
            }
            if (contact.ConfirmKey.Value == Key)
            {
                RemoveKey(contact, Data);
                return KeyStatus.Success;
            }
            return KeyStatus.Fail;
        }

        public DateTime Generate(Contact contact, IUOW Data)
        {
            if (contact == null) throw new KeyException(KeyError.ContactNotFound);
            if (contact.ConfirmKey != null)
            {
                if (contact.ConfirmKey.EndTime > DateTime.Now)
                {
                    return contact.ConfirmKey.EndTime;
                }
            }
            contact.ConfirmKey = KeyGenerator.Get();
            Data.ContactRepository.Edited(contact);
            Data.SaveChanges();
            Messager.Send(contact.Value, "Your key: " + contact.ConfirmKey.Value);
            return contact.ConfirmKey.EndTime;
        }
    }
}
