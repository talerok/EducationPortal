using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.UserServices.Interfaces
{
    public interface IKeyGenerator
    {
        Key Get();
    }
}
