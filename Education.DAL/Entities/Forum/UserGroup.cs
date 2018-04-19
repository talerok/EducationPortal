using System;
using System.Collections.Generic;
using System.Text;

namespace Education.DAL.Entities
{
    public enum UserGroupStatus
    {
        request,
        member,
        owner
    }
    public class UserGroup
    {
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public UserGroupStatus Status { get; set; }
    }
}
