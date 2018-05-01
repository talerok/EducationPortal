using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Education.BLL.DTO;
using Education.BLL.DTO.Forum;
using Education.BLL.DTO.User;
using Education.BLL.Services.ImageManager.Interfaces;
using Education.BLL.Services.UserServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Education.Controllers
{
    public class ImageManagerController : Controller
    {
        private IClaimService ClaimService;
        private IImageService ImageService;

        public ImageManagerController(IClaimService claimService, IImageService imageService)
        {
            ClaimService = claimService;
            ImageService = imageService;
        }

        private UserDTO GetUser()
        {
            return ClaimService.GetUser(User.Claims);
        }

        [HttpPost]
        public IActionResult Load()
        {
            var user = GetUser();
            if (!ImageService.CanLoad(user)) return new UnauthorizedResult();
            foreach(var file in Request.Form.Files)
            {
                if (!ImageService.CheckFile(new FileDTO { Size = file.Length, Type = Path.GetExtension(file.FileName) }))
                    continue;
                file.CopyTo(new FileStream(ImageService.ImageFolder + file.FileName, FileMode.CreateNew));
            }
            return new OkResult();
        }

        [HttpPost]
        public IActionResult Delete(string path)
        {
            var res = ImageService.Delete(path, GetUser());
            if (res == AccessCode.NoPremision) return new UnauthorizedResult();
            if (res == AccessCode.NotFound) return new NotFoundObjectResult(path);
            if (res == AccessCode.Error) return new BadRequestResult();
            return new OkResult();

        }

        public IActionResult Images()
        {
            return PartialView(ImageService.Get());
        }

    }
}