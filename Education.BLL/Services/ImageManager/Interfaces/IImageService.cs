using Education.BLL.DTO;
using Education.BLL.DTO.Forum;
using Education.BLL.DTO.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Services.ImageManager.Interfaces
{
    public interface IImageService
    {
        string ImageFolder { get; }
        IEnumerable<string> AllowedImgTypes { get; }
        bool CanLoad(UserDTO userDTO);
        bool CheckFile(FileDTO fileDTO);
        string AvatarPath(UserDTO userDTO);
        IEnumerable<ImageInfoDTO> Get();
        AccessCode Delete(string path, UserDTO user);
        bool CanDelete(UserDTO userDTO);
    }
}
