using Education.BLL.DTO.Forum;
using Education.BLL.DTO.User;
using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.ForumServices.Interfaces
{
    public interface IControlService<D,T> 
    {
        CreateResultDTO Create(T DTO, UserDTO userDTO);

        AccessCode Update(T DTO, UserDTO userDTO);

        AccessCode Delete(int id, UserDTO userDTO);

        (AccessCode, D) Read(int id, UserDTO userDTO);
        
    }
}
