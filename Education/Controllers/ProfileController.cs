using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Education.BLL.DTO.User;
using Education.BLL.DTO;
using Education.BLL.Services.UserServices.Interfaces;
using Education.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Education.BLL.Services.ImageManager.Interfaces;

namespace Education.Controllers
{
    public class ProfileController : Controller
    {
        private IProfileService ProfileService;
        private IImageService ImageService;
        private UserDTO GetUser()
        {
            return ProfileService.ClaimService.GetUser(User.Claims);
        }

        [Authorize]
        public IActionResult Index()
        {
            return View(ProfileService.GetUserProfile(GetUser()));
        }

        public ProfileController(IProfileService profileService, IImageService imageService)
        {
            ProfileService = profileService;
            ImageService = imageService;
        }

        [Authorize]
        [HttpPost]
        [Route("api/ConfirmEmail")]
        public ConfirmResult ConfirmEmail(string key = null)
        {
            return ProfileService.ConfirmEmail(GetUser(),key);
        }

        [Authorize]
        [HttpPost]
        [Route("api/ConfirmPhone")]
        public ConfirmResult ConfirmPhone(string key = null)
        {
            return ProfileService.ConfirmPhone(GetUser(),key);
        }

        [Authorize]
        [HttpPost]
        [Route("api/RemovePhone")]
        public ConfirmResult RemovePhone(string key = null)
        {
            return ProfileService.RemovePhone(GetUser(),key);
        }
        [HttpPost]
        [Route("api/RemoveEmail")]
        public ConfirmResult RemoveEmail(string key = null)
        {
            return ProfileService.RemoveEmail(GetUser(), key);
        }

        [Authorize]
        [HttpPost]
        [Route("api/SetEmail")]
        public SetContactCode SetEmail(string value)
        {
            return ProfileService.SetEmail(GetUser(), value);
        }

        [Authorize]
        [HttpPost]
        [Route("api/SetPhone")]
        public SetContactCode SetPhone(string value)
        {
            return ProfileService.SetPhone(GetUser(), value);
        }

        [Authorize]
        [HttpPost]
        [Route("api/ResetAuthType")]
        public ConfirmResult ResetAuthType(string key = null)
        {
            return ProfileService.ResetAuthType(GetUser(), key);
        }

        [Authorize]
        [HttpPost]
        [Route("api/SetAuthType")]
        public ConfirmResult SetAuthType(AuthType value, string key = null)
        {
            return ProfileService.SetAuthType(GetUser(), value, key);
        }

        [Authorize]
        [HttpPost]
        [Route("api/SetPassword")]
        public ConfirmResult SetPassword(string oldpassword, string newPassword, string key = null)
        {
            return ProfileService.SetPassword(GetUser(), oldpassword, newPassword, key);
        }

        [Authorize]
        [HttpPost]
        public IActionResult SetAvatar(IFormFile file)
        {
            var user = GetUser();
            if (file == null || user == null) return Redirect("/");

            var type = Path.GetExtension(file.FileName);

            if (!ImageService.CheckFile(new FileDTO { Size = file.Length, Type = type }))
                return RedirectToAction("Index");
            var path = ImageService.AvatarPath(user);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            if (path != null) path+= Path.GetRandomFileName() + type;
            file.CopyTo(new FileStream(path, FileMode.Create));
            ProfileService.SetAvatar(user, path.Replace(@"wwwroot", ""));
            return RedirectToAction("Index");
        }

    }
}