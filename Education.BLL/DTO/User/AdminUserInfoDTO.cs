using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.User
{
    public class AdminUserInfoDTO : UserInfoDTO
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public short Level { get; set; }
        public bool Banned { get; set; } 
        public string BanReason { get; set; }
        public DateTime? BanTime { get; set; }
    }
}
