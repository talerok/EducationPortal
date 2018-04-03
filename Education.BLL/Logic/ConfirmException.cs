using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Logic
{
    public enum ConfirmError
    {
        NotFound,
        AlreadyConfirmed
    }

    public class ConfirmException : Exception
    {
        public ConfirmError Status { get; private set; }

        public ConfirmException(ConfirmError error) : base()
        {
            Status = error;
        }
    }
}
