using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Education.BLL.DTO.Forum;
using Education.BLL.DTO.Pages;
using Education.BLL.DTO.User;
using Education.BLL.Services.PageServices.Interfaces;
using Education.BLL.Services.UserServices.Interfaces;
using Education.Controllers.Base;
using Education.Models;
using Microsoft.AspNetCore.Mvc;

namespace Education.ControllersPageService
{
    public class PageController : DefaultController
    {
        private IPageService PageService;
        private IClaimService ClaimService;

        public PageController(IPageService pageService, IClaimService claimService)
        {
            PageService = pageService;
            ClaimService = claimService;
        }

        private UserDTO GetUser()
        {
            return ClaimService.GetUser(User.Claims);
        }

        public IActionResult All()
        {
            if (PageService.CanCreate(GetUser()))
                return View(PageService.Map.Get);
            else return Redirect(AccessCode.NoPremision);
        }

        public IActionResult Index(int id)
        {
            var res = PageService.Get(id, GetUser());
            if (res.Item1 == AccessCode.Succsess) return View(res.Item2);
            return Redirect(res.Item1);
        }

        public IActionResult Control(int id = -1)
        {
            var user = GetUser();
            if(id == -1)
            {
                if (PageService.CanCreate(user))
                    return View(new PageControl { PageDTO = null, Map = PageService.Map.Get });
                else return Redirect(AccessCode.NoPremision);
            }
            var page = PageService.Get(id, user);
            if(page.Item1 == AccessCode.Succsess && page.Item2.Access.CanUpdate)
                return View(new PageControl { PageDTO = page.Item2, Map = PageService.Map.Get });
            return Redirect(page.Item1);
        }

        [HttpPost]
        public IActionResult Create(PageEditDTO pageEditDTO)
        {
            var res = PageService.Create(pageEditDTO, GetUser());
            if (res.Code == AccessCode.Succsess)
                return RedirectToAction("index", new { id = res.Id });
            return Redirect(res.Code);
        }

        [HttpPost]
        public IActionResult Edit(PageEditDTO pageEditDTO)
        {
            var res = PageService.Update(pageEditDTO, GetUser());
            if (res == AccessCode.Succsess)
                return RedirectToAction("index", new { id = pageEditDTO.Id });
            return Redirect(res);
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            return Redirect(PageService.Delete(id, GetUser()));
        }
    }
}