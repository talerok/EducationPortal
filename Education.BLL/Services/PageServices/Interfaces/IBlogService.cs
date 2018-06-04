using Education.BLL.DTO.Forum;
using Education.BLL.DTO.Pages;
using Education.BLL.DTO.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.PageServices.Interfaces
{
    public interface IBlogService
    {
        CreateResultDTO Create(NoteEditDTO noteEditDTO, UserDTO userDTO);
        AccessCode Delete(int id, UserDTO userDTO);
        AccessCode Update(NoteEditDTO noteEditDTO, UserDTO userDTO);
        BlogDTO Get(UserDTO userDTO, int page);
        (AccessCode, NoteDTO) Get(int id, UserDTO userDTO);
        bool CanCreate(UserDTO userDTO);
    }
}
