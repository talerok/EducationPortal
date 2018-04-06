using Education.BLL.DTO.User;
using Education.BLL.DTO.Forum;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.ForumService.Interfaces
{
    public enum SimpleResult
    {
        succsess,
        notFound,
        noPremission
    }

    interface IThemeService
    {
        void AddMessage(UserDTO userDTO, int themeId, string text);
        void DeleteMessage(UserDTO userDTO, int themeId, int messageId);
        void ChangeMessage(UserDTO userDTO, int themeId, int messageId, string text);
        void DeleteTheme(UserDTO userDTO, int themeId);
        void ChangeDescription(UserDTO userDTO, int themeId, string Description);
        void ChangeName(UserDTO userDTO, int themeId, string name);
        IEnumerable<MessageDTO> GetMessages(int themeId, UserDTO userDTO);
    }
}
