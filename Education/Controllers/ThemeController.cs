using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Education.BLL.DTO.Forum;
using Education.BLL.DTO.Forum.Edit;
using Education.BLL.DTO.User;
using Education.BLL.Services.ForumServices.Interfaces;
using Education.BLL.Services.UserServices.Interfaces;
using Education.Controllers.Base;
using Education.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Education.Controllers
{
    public class ThemeController : DefaultController
    {
        private IThemeService ThemeService;
        private IClaimService ClaimService;

        public ThemeController(IThemeService themeService, IClaimService claimService)
        {
            ThemeService = themeService;
            ClaimService = claimService;
        }

        private UserDTO GetUser()
        {
            return ClaimService.GetUser(User.Claims);
        }

        public IActionResult Index(int id, int page = 1)
        {
            var res = ThemeService.Read(id, page, GetUser());
            if (res.Item1 == AccessCode.Succsess) return View(res.Item2);
            else return Redirect(res.Item1);
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            var res = ThemeService.Delete(id, GetUser());
            return Redirect(res);
        }

        [HttpPost]
        public IActionResult Create(ThemeRequest themeRequest)
        {
            var theme = new ThemeEditDTO
            {
                Name = themeRequest.Name,
                SectionId = themeRequest.SectionId,
                Open = themeRequest.Open,
                Text = themeRequest.Text
            };

            var res = ThemeService.Create(theme, GetUser());
            if (res.Code == AccessCode.Succsess)
                return RedirectToAction("Index", new { id = res.Id });
            else return Redirect(res.Code);
        }

        [HttpPost]
        public IActionResult Update(ThemeRequest themeRequest)
        {
            var theme = new ThemeEditDTO
            {
                Name = themeRequest.Name,             
                Open = themeRequest.Open,
                Text = themeRequest.Text,
                Id = themeRequest.Id
            };

            var res = ThemeService.Update(theme, GetUser());
            if (res == AccessCode.Succsess)
                return RedirectToAction("Index", new { id = themeRequest.Id });
            else return Redirect(res);
        }

        public IActionResult Control(int id)
        {
            var res = ThemeService.Read(id, GetUser());
            if(res.Item1 == AccessCode.Succsess && res.Item2.Access.CanUpdate)
            {
                return View(new ThemeControl(-1, res.Item2));
            }
            return Redirect(res.Item1);
        }

        public IActionResult Add(int id)
        {
            if (ThemeService.CanCreate(id, GetUser()))
                return View("Control", new ThemeControl(id, null));
            else return Redirect(AccessCode.NoPremision);
        }

    }
}