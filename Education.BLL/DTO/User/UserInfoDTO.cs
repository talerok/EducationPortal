using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.User
{
    public class UserInfoDTO
    {
        public string Avatar { get; set; }
        public string email { get; set; }
        public bool? emailConfirm { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        public bool? phoneConfirm { get; set; }
        public AuthType authType { get; set; }
    }
}
