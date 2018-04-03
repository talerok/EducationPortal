using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.User
{
    public class ClaimInfoDTO
    {
        public DateTime LoginTime { get; set; }
        public string Ip { get; set; }
        public string Browser { get; set; }
    }
}
