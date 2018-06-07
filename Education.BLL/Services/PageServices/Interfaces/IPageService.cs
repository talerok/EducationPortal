using Education.BLL.DTO.Forum;
using Education.BLL.DTO.Pages;
using Education.BLL.DTO.User;
using Education.BLL.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.PageServices.Interfaces
{
    public interface IPageService
    {
        bool CanCreate(UserDTO userDTO);
        IPageMap Map { get; }
        (AccessCode, PageDTO) Get(int id, UserDTO userDTO);
        AccessCode Update(PageEditDTO pageEditDTO, UserDTO userDTO);
        AccessCode Delete(int id, UserDTO userDTO);
        CreateResultDTO Create(PageEditDTO pageEditDTO, UserDTO userDTO);
    }
}
