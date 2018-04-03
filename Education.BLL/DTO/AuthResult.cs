using Education.BLL.Services.UserServices.Interfaces;
using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Education.BLL.DTO
{
    public class AuthResult
    {
        public AuthStatus Status { get; set; }
        public AuthType authType { get; set; }
        public DateTime KeyTime { get; set; }
        public ClaimsIdentity Identity { get; set; }
        public string Comment { get; set; }
    
    }
}
