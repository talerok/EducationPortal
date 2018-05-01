using Education.BLL.DTO.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Education.Models
{
    public class UsersOfGroup
    {
        public int GroupId { get; set; }
        public IEnumerable<UserGroupDTO> Data { get; set; }
    }
}
