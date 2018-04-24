using Education.BLL.DTO.User;
using Education.BLL.Logic.Interfaces;
using Education.DAL.Entities;
using Education.DAL.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Logic
{
    public class GetUserDTO : IGetUserDTO
    {
        public User Get(UserDTO userDTO, IUOW Data)
        {
            if (userDTO == null) return null;
            var name = userDTO.Login.ToLower();
            return Data.UserRepository.Get().FirstOrDefault(x => x.Login == name
            && x.Password == userDTO.Password);
        }
    }
}
