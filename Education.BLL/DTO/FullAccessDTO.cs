using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO
{
    public class FullAccessDTO : AccessDTO
    {
        public bool CanCreateElements{ get; set; }
    }
}
