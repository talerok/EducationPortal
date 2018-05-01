using Education.BLL.DTO.Forum;
using Education.BLL.DTO.Forum.Edit;
using Education.BLL.DTO.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.ForumServices.Interfaces
{
    public interface ISectionService : IControlService<SectionDTO, SectionEditDTO>
    {
        bool CanCreate(int GroupId, UserDTO userDTO);
    }
}
