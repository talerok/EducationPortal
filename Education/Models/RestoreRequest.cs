﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Education.Models
{
    public class RestoreRequest
    {
        public string Login { get; set; }
        public string Key { get; set; }
        public string Password { get; set; }
    }
}
