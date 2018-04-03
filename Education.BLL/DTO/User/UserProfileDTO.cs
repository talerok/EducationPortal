using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.User
{
    public class UserProfileDTO
    {
        public string email { get; set; }
        public bool? emailConfirm { get; set; }
        public string phone { get; set; }
        public bool? phoneConfirm { get; set; }
        public AuthType authType { get; set; }
        public IEnumerable<ClaimInfoDTO> Claims { get; set; }
    }
}
