using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Logic
{
    public enum KeyError
    {
        ContactNotFound,
        KeyNotFound
    }
    public class KeyException : Exception
    {
        public KeyError Status { get; private set; }
        public KeyException(KeyError status) : base()
        {
            Status = status;
        }
    }
}
