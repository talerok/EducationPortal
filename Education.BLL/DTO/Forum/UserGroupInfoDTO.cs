using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Forum
{
    public class UserGroupInfoDTO
    {
        public static UserGroupInfoDTO NoPremision
        {
            get
            {
                return new UserGroupInfoDTO { Code = AccessCode.NoPremision};
            }
        }

        public static UserGroupInfoDTO NotFound
        {
            get
            {
                return new UserGroupInfoDTO { Code = AccessCode.NotFound };
            }
        }

        public static UserGroupInfoDTO Error
        {
            get
            {
                return new UserGroupInfoDTO { Code = AccessCode.Error };
            }
        }

        public AccessCode Code { get; set; }
        public IEnumerable<UserGroupDTO> Data { get; set; }
    }
}
