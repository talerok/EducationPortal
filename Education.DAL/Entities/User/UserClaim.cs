using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Education.DAL.Entities
{
    public class UserClaim : Entity
    {
        public virtual User User { get; set; }
        public string Value { get; set; }
        public DateTime LoginTime { get; set; }
        public string LoginBrowser { get; set; }
        public string LoginIp { get; set; }
    }
}