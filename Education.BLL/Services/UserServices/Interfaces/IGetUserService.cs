using Education.BLL.DTO.User;
using Education.DAL.Entities;
using Education.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.UserServices.Interfaces
{
    interface IGetUserService
    {
        User Get(UserDTO userDTO, IUOW Data);
        User Get(string login, IUOW Data);
    }
}
