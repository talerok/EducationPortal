using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.User
{
    public class LoginInfoDTO
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Key { get; set; }
        public string Browser { get; set; }
        public string IP { get; set; }
    }
}
