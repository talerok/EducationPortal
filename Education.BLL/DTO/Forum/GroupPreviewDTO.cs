using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Forum
{
    public class UserGroupInfo
    {
        public bool Member { get; set; }
        public UserGroupStatus Status { get; set; }
    }

    public class GroupPreviewDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public bool Open { get; set; }
        public GroupAccessDTO Access { get; set; }
        public UserGroupInfo Status { get; set; }
    }
}
