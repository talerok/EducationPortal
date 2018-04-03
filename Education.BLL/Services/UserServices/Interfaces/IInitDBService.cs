using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Education.BLL.Services.UserServices.Interfaces
{
    public interface IInitDBService
    {
        void InitAdmin(string login, string password);
    }
}