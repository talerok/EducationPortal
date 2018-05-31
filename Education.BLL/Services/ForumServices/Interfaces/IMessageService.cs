using Education.BLL.DTO.Forum;
using Education.BLL.DTO.Forum.Edit;
using Education.BLL.DTO.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.ForumServices.Interfaces
{
    public interface IMessageService : IControlService<MessageDTO, MessageEditDTO>
    {
        bool CanCreate(int ThemeId, UserDTO userDTO);

        MessagePreviewDTO Get(int id, UserDTO userDTO);
    }
}
