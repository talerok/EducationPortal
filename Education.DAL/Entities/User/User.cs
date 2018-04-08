
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Education.DAL.Entities
{
    public enum AuthType
    {
        Simple,
        Email,
        Phone
    }

    public class User : Entity
    {
        public string Login { get; set; }
        public virtual Contact Email { get; set; }
        public virtual Contact Phone { get; set; }
        public string Password { get; set; }
        public virtual UserInfo Info { get; set; }
        public short Level { get; set; }
        public AuthType authType { get; set; }
        public virtual Ban Ban { get; set;}
        public virtual ICollection<UserGroup> Groups { get; set; }
        public User() { }
    }
}
