using Education.BLL.DTO;
using Education.BLL.DTO.Forum;
using Education.BLL.DTO.User;
using Education.BLL.Logic;
using Education.BLL.Logic.Interfaces;
using Education.BLL.Services.ImageManager.Interfaces;
using Education.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Education.BLL.Services.ImageManager
{
    public class ImageService : IImageService
    {
        private IUOWFactory DataFactory;
        private IGetUserDTO GetUserService;
        private long MaxSize;

        public string ImageFolder { get; private set; } = @"wwwroot/images/";

        public IEnumerable<string> AllowedImgTypes { get; private set; }
            = new string[] { ".jpg", ".jpeg", ".bmp", ".png" };

        public ImageService(IUOWFactory dataFactory, IGetUserDTO getUserDTO, long maxSize = 1000000)
        {
            DataFactory = dataFactory;
            GetUserService = getUserDTO;
            MaxSize = maxSize;
        }

        public bool CanLoad(UserDTO userDTO)
        {
            using(var Data = DataFactory.Get())
            {
                var user = GetUserService.Get(userDTO, Data);
                if (user == null || user.Level < 1) return false;
                else return true;
            }
        }

        public string AvatarPath(UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var user = GetUserService.Get(userDTO, Data);
                if (user == null) return null;
                else return ImageFolder + user.Login + "/";
            }
        }

        private bool CheckType(string type)
        {
            foreach(var a in AllowedImgTypes)
                if (a == type.ToLower()) return true;
            return false;
        }

        public bool CheckFile(FileDTO fileDTO)
        {
            if (fileDTO.Size > MaxSize) return false;
            return CheckType(fileDTO.Type);
        }

        public bool CanDelete(UserDTO userDTO)
        {
            return CanLoad(userDTO);
        }

        public AccessCode Delete(string path, UserDTO user)
        {
            if (!CanDelete(user)) return AccessCode.NoPremision;
            string realPath = "wwwroot" + path;
            if (!File.Exists("wwwroot" + path)) return AccessCode.NotFound;
            try
            {
                File.Delete(realPath);
                return AccessCode.Succsess;
            }
            catch
            {
                return AccessCode.Error;
            }
        } 

        public IEnumerable<ImageInfoDTO> Get()
        {
            var Images = new List<ImageInfoDTO>();

            foreach (var image in Directory.GetFiles(ImageFolder))
            {
                var fileInfo = new FileInfo(image);
                if (!CheckFile(new FileDTO { Size = fileInfo.Length, Type = fileInfo.Extension }))
                    continue;
                Images.Add(new ImageInfoDTO { Path = image.Replace(@"wwwroot", ""), Size = fileInfo.Length });

            }
            return Images;
        }
    }
}
