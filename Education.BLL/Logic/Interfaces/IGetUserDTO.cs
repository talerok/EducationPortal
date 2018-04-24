﻿using Education.BLL.DTO.User;
using Education.DAL.Entities;
using Education.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Logic.Interfaces
{
    public interface IGetUserDTO
    {
        User Get(UserDTO userDTO, IUOW Data);
    }
}
