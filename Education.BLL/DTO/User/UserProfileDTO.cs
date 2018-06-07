using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.User
{
    public class UserProfileDTO : UserInfoDTO
    {
        public IEnumerable<ClaimInfoDTO> Claims { get; set; }
    }
}
