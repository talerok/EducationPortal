using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO
{
    public enum RestoreCode
    {
        Succsess,
        KeySent,
        WorngKey,
        UserNotFound,
        NeedContact,
        WrongPassword,
        NeedNewKey
    };
    public class RestoreResult
    {
        public RestoreCode Status { get; set; }
        public AuthType? SendTo { get; set; }
        public DateTime KeyTime { get; set; }
    }
}
