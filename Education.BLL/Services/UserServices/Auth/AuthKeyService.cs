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
        private IUOW Data;

        public AuthKeyService(IUOW data, IKeyGenerator kg, IMessenger msgr)
        {
            Messager = msgr;
            Data = data;
            KeyGenerator = kg;
        }

        private void RemoveKey(Contact contact)
        {
            Data.AuthKeyRepository.Delete(contact.Key);
        }

        public KeyStatus Check(Contact contact, string Key)
        {
            if (contact == null) throw new KeyException(KeyError.ContactNotFound);
            if (contact.Key == null) throw new KeyException(KeyError.KeyNotFound);
            if (contact.Key.EndTime < DateTime.Now)
            {
                RemoveKey(contact);
                return KeyStatus.KeyTimeEnded;
            }
            if (contact.Key.Value == Key)
            {
                RemoveKey(contact);
                return KeyStatus.Success;
            }
            return KeyStatus.Fail;
        }

        public DateTime Generate(Contact contact)
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
            Messager.Send(contact.Value, "Your key: " + contact.Key.Value);
            return contact.Key.EndTime;
        }
    }
}