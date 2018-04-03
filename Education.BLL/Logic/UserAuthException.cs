using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Logic
{
    public enum AuthError
    {
        UserNotFound,
        NeedNewKey,
        WrongKey,
        KeyAlreadySet,
        AuthTypeNotFound
    }
    public class UserAuthException : Exception
    {
        public AuthError Status { get; private set; }
        public UserAuthException(AuthError status) : base()
        {
            Status = status;
        }
    }
}
