using Education.BLL.DTO.Forum;
using Education.BLL.DTO.Forum.Edit;
using Education.BLL.DTO.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.ForumServices.Interfaces
{
    public interface IThemeService : IControlService<ThemeDTO, ThemeEditDTO>
    {
        bool CanCreate(int SectionId, UserDTO userDTO);
        (AccessCode, ThemeDTO) Read(int id, int page, UserDTO userDTO);
    }
}
