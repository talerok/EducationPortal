using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Education.BLL.DTO.Forum;
using Education.BLL.DTO.User;
using Education.BLL.Services.AdminService.Interfaces;
using Education.BLL.Services.ConfigService.Interfaces;
using Education.BLL.Services.UserServices.Interfaces;
using Education.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Education.Controllers
{
    public class AdminController : Base.DefaultController
    {
        private IClaimService ClaimService;
        private IAdminService AdminService;
        private IConfigService ConfigService;
        private UserDTO GetUser()
        {
            return ClaimService.GetUser(User.Claims);
        }

        public AdminController(IClaimService claimService, IAdminService adminService, IConfigService configService)
        {
            ClaimService = claimService;
            AdminService = adminService;
            ConfigService = configService;
        }

        public IActionResult Index()
        {
            if (AdminService.IsAdmin(GetUser()))
                return View();
            else return Redirect(AccessCode.NoPremision);
        }

        #region Users

        public IActionResult Users()
        {
            if (AdminService.IsAdmin(GetUser()))
                return View();
            else return Redirect(AccessCode.NoPremision);
        }

        public IActionResult UserInfo(int id)
        {
            var user = AdminService.GetUser(GetUser(), id);
            if (user.Code != AccessCode.Succsess)
                return Redirect(user.Code);
            return View(user.Result);
        }

        [HttpPost]
        public IActionResult EditUser(AdminUserInfoDTO dto)
        {
            var res = AdminService.EditUser(GetUser(), dto);
            if (res == AccessCode.Succsess)
                return RedirectToAction("Users");
            return Redirect(res);
        }

        [HttpPost]
        public IActionResult ClearUserSessions(int id)
        {
            var res = AdminService.ResetClaims(GetUser(), id);
            if (res == AccessCode.Succsess)
                return RedirectToAction("Users");
            return Redirect(res);
        }

        [HttpPost]
        public IActionResult SearchUsers(string search = null)
        {
            var user = GetUser();
            if (!AdminService.IsAdmin(user))
                return Redirect(AccessCode.NoPremision);
            IEnumerable<AdminUserInfoDTO> users;
            AccessCode code;
            if(search == null)
            {
                var res = AdminService.GetAll(user);
                code = res.Code;
                users = res.Result;
            }
            else
            {
                var res = AdminService.Search(user, search);
                code = res.Code;
                users = res.Result;
            }
            if (code != AccessCode.Succsess)
                return ErrorCode(code);

            return PartialView(users);

        }

        #endregion

        #region Config
        public IActionResult Config()
        {
            if (AdminService.IsAdmin(GetUser()))
                return View(new Config { ConnString = ConfigService.ConnectionString, Icon = ConfigService.IconPath, Name = ConfigService.Name });
            else return Redirect(AccessCode.NoPremision);
        }

        public async Task<IActionResult> ChangeIcon(IFormFile uploadedFile)
        {
            if (!AdminService.IsAdmin(GetUser()))
                return Redirect(AccessCode.NoPremision);
            using (var fileStream = new FileStream(ConfigService.IconPath, FileMode.Create))
                await uploadedFile.CopyToAsync(fileStream);
            return RedirectToAction("Index");
        }

        public IActionResult ChangeConfig(Config config)
        {
            if (!AdminService.IsAdmin(GetUser()))
                return Redirect(AccessCode.NoPremision);

            ConfigService.ConnectionString = config.ConnString;
            ConfigService.Name = config.Name;
            return RedirectToAction("Index");
        }

        #endregion
    }
}