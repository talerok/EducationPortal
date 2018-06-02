using Education.BLL.DTO;
using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Logic.Interfaces
{
    public interface IDTOHelper
    {
        UserPublicInfoDTO GetUser(User user);
    }
}
