using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Forum
{
    public class GroupAccessDTO : FullAccessDTO
    {
        public bool CanControlUsers { get; set; }
    }
}
