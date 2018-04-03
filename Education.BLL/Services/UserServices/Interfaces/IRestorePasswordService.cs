using Education.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.UserServices.Interfaces
{
    public interface IRestorePasswordService
    {
        RestoreResult Restore(string login, string password = null, string key = null);
    }
}
