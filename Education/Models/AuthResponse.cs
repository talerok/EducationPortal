using Education.BLL.Services.UserServices.Interfaces;
using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Education.Models
{
    public class AuthResponse
    {
        public AuthStatus Status { get; set; }
        public AuthType AuthType { get; set; }
        public DateTime? KeyTime { get; set; }
    }
}
