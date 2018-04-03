using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Logic
{
    class DBInitException : Exception
    {
        public DBInitException(string reason) : base(reason)
        {

        }
    }
}
