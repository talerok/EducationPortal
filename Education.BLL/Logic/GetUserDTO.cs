using Education.BLL.DTO.User;
using Education.BLL.Logic.Interfaces;
using Education.DAL.Entities;
using Education.DAL.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;

namespace Education.BLL.Logic
{
    public class GetUserDTO : IGetUserDTO
    {
        private bool CheckBan(User user)
        {
            if (user.Ban == null) return false;
            if (user.Ban.EndTime < DateTime.Now) return true;
            else return false;
        }

        public User Get(UserDTO userDTO, IUOW Data)
        {
            if (userDTO == null) return null;
            var user = Data.UserRepository.Get().FirstOrDefault(x => x.Id == userDTO.Id);
            if (CheckBan(user)) return null;
            return user;
        }
    }
}
