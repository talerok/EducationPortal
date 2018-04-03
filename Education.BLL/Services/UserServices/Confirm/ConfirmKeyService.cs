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
        private IUOW Data;

        public ConfirmKeyService(IUOW data, IKeyGenerator kg, IMessenger msgr)
        {
            Messager = msgr;
            Data = data;
            KeyGenerator = kg;
        }

        public KeyStatus Check(Contact contact, string Key)
        {
            if (contact == null) throw new KeyException(KeyError.ContactNotFound);
            if (contact.ConfirmKey == null) return KeyStatus.Fail;
            if (contact.ConfirmKey.EndTime < DateTime.Now)
            {
                Data.AuthKeyRepository.Delete(contact.ConfirmKey);
                return KeyStatus.KeyTimeEnded;
            }
            if (contact.ConfirmKey.Value == Key)
            {
                Data.AuthKeyRepository.Delete(contact.ConfirmKey);
                return KeyStatus.Success;
            }
            return KeyStatus.Fail;
        }

        public DateTime Generate(Contact contact)
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
            Messager.Send(contact.Value, "Your key: " + contact.ConfirmKey.Value);
            return contact.ConfirmKey.EndTime;
        }
    }
}
