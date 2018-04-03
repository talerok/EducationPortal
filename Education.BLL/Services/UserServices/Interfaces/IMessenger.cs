using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.UserServices.Interfaces
{
    public interface IMessenger
    {
        void Send(string to, string text);
    }
}
