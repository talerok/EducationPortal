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

namespace Education.BLL.Services.UserServices.Auth
{
    public class AuthKeyService : IAuthKeyService
    {
        private IMessenger Messager;
        private IKeyGenerator KeyGenerator;

        public AuthKeyService(IKeyGenerator kg, IMessenger msgr)
        {
            Messager = msgr;
            KeyGenerator = kg;
        }

        private void RemoveKey(Contact contact, IUOW Data)
        {
            Data.AuthKeyRepository.Delete(contact.Key);
            Data.SaveChanges();
        }

        public KeyStatus Check(Contact contact, string Key, IUOW Data)
        {
            if (contact == null) throw new KeyException(KeyError.ContactNotFound);
            if (contact.Key == null) throw new KeyException(KeyError.KeyNotFound);
            if (contact.Key.EndTime < DateTime.Now)
            {
                RemoveKey(contact, Data);
                return KeyStatus.KeyTimeEnded;
            }
            if (contact.Key.Value == Key)
            {
                RemoveKey(contact, Data);
                return KeyStatus.Success;
            }
            return KeyStatus.Fail;
        }

        public DateTime Generate(Contact contact, IUOW Data)
        {
            if (contact == null) throw new KeyException(KeyError.ContactNotFound);
            if (contact.Key != null)
            {
                if (contact.Key.EndTime > DateTime.Now)
                {
                    return contact.Key.EndTime;
                }
            }
            contact.Key = KeyGenerator.Get();
            Data.ContactRepository.Edited(contact);
            Data.SaveChanges();
            Messager.Send(contact.Value, "Your key: " + contact.Key.Value);
            return contact.Key.EndTime;
        }
    }
}