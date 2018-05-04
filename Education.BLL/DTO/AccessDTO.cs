using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO
{
    public class AccessDTO
    {
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
    }
}
