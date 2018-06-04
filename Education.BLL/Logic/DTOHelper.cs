using Education.BLL.DTO;
using Education.BLL.Logic.Interfaces;
using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Logic
{
    public class DTOHelper : IDTOHelper
    {
        public UserPublicInfoDTO GetUser(User user)
        {
            if (user == null) return null;
            return new UserPublicInfoDTO
            {
                AvatarPath = user.Info.Avatar,
                Id = user.Id,
                Name = user.Info.FullName
            };
        }
    }
}
