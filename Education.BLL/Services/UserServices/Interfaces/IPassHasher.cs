using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.UserServices.Interfaces
{
    public interface IPassHasher
    {
        string Get(string input);
    }
}
