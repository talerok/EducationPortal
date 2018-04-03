using Education.BLL.Services.UserServices.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Education.BLL.DTO
{
    public class ConfirmResult
    {
        public ConfirmCode Status { get; set; }
        public DateTime KeyTime { get; set; }
    }
}