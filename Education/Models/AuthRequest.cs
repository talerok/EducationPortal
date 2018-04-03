using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Education.Models
{
    public class AuthRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string SecretKey { get; set; }

    }
}
