using Education.BLL.DTO.Forum;
using Education.BLL.DTO.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.ForumServices.Interfaces
{
    public interface IForumService
    {
        ForumDTO Get(UserDTO userDTO);
    }
}
