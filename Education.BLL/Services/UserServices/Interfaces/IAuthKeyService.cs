using Education.BLL.DTO.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Education.DAL.Entities;

namespace Education.BLL.Services.UserServices.Interfaces
{
    public enum KeyStatus
    {
        Success,
        Fail,
        KeyTimeEnded
    }


    public interface IAuthKeyService
    {
        DateTime Generate(Contact contact);
        KeyStatus Check(Contact contact, string Key);
    }
}