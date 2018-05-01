using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Forum
{
    public class UserGroupDTO
    {
        public UserPublicInfoDTO UserInfo { get; set; }
        public UserGroupStatus Status { get; set; }
    }
}
