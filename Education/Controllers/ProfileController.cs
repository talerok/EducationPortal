using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Education.BLL.DTO.User;
using Education.BLL.DTO;
using Education.BLL.Services.UserServices.Interfaces;
using Education.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Education.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private IProfileService ProfileService;

        private UserDTO GetUser()
        {
            return ProfileService.ClaimService.GetUser(User.Claims);
        }

        public IActionResult Index()
        {
            return View(ProfileService.GetUserProfile(GetUser()));
        }

        public ProfileController(IProfileService profileService)
        {
            ProfileService = profileService;
        }

        [HttpPost]
        [Route("api/ConfirmEmail")]
        public ConfirmResult ConfirmEmail(string key = null)
        {
            return ProfileService.ConfirmEmail(GetUser(),key);
        }
        [HttpPost]
        [Route("api/ConfirmPhone")]
        public ConfirmResult ConfirmPhone(string key = null)
        {
            return ProfileService.ConfirmPhone(GetUser(),key);
        }
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
        [HttpPost]
        [Route("api/SetEmail")]
        public SetContactCode SetEmail(string value)
        {
            return ProfileService.SetEmail(GetUser(), value);
        }
        [HttpPost]
        [Route("api/SetPhone")]
        public SetContactCode SetPhone(string value)
        {
            return ProfileService.SetPhone(GetUser(), value);
        }
        [HttpPost]
        [Route("api/ResetAuthType")]
        public ConfirmResult ResetAuthType(string key = null)
        {
            return ProfileService.ResetAuthType(GetUser(), key);
        }
        [HttpPost]
        [Route("api/SetAuthType")]
        public ConfirmResult SetAuthType(AuthType value, string key = null)
        {
            return ProfileService.SetAuthType(GetUser(), value, key);
        }
    
        [HttpPost]
        [Route("api/SetPassword")]
        public ConfirmResult SetPassword(string oldpassword, string newPassword, string key = null)
        {
            return ProfileService.SetPassword(GetUser(), oldpassword, newPassword, key);
        }


    }
}