using Education.BLL.DTO.Forum;
using Education.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace Education.BLL.Logic.Interfaces
{
    public interface IForumDTOHelper
    {
        GroupDTO GetDTO(Group group, User user);
        SectionDTO GetDTO(Section section, User user);
        ThemeDTO GetDTO(Theme theme, User user, int page);
        ThemeDTO GetDTO(Theme theme, User user);
        MessageDTO GetDTO(Message message, User user);
        GroupPreviewDTO GetDTO(User user, Group group);
        UserGroupDTO GetDTO(UserGroup userGroup);
        MessagePreviewDTO GetDTO(Message message);
    }
}
