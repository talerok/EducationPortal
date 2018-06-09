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
using Microsoft.AspNetCore.Mvc;

namespace Education.Controllers
{
    public class SectionController : DefaultController
    {
        private ISectionService SectionService;
        private IClaimService ClaimService;

        public SectionController(ISectionService sectionService, IClaimService claimService)
        {
            SectionService = sectionService;
            ClaimService = claimService;
        }
    

        private UserDTO GetUser()
        {
            return ClaimService.GetUser(User.Claims);
        }

        public IActionResult Index(int id)
        {
            var res = SectionService.Read(id, GetUser());
            if (res.Item1 == AccessCode.Succsess) return View(res.Item2);
            else return Redirect(res.Item1);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var res = SectionService.Delete(id, GetUser());
            return Redirect(res);
        }

        [HttpPost]
        public IActionResult Create(SectionRequest sectionRequest)
        {
            var section = new SectionEditDTO
            {
                Name = sectionRequest.Name,
                Open = sectionRequest.Open,
                GroupId = sectionRequest.GroupId
            };

            var res = SectionService.Create(section, GetUser());
            if (res.Code == AccessCode.Succsess)
                return RedirectToAction("Index", new { id = res.Id });
            else return Redirect(res.Code);
        }

        [HttpPost]
        public IActionResult Update(SectionRequest sectionRequest)
        {
            var section = new SectionEditDTO
            {
                Name = sectionRequest.Name,
                Open = sectionRequest.Open,
                Id = sectionRequest.Id
            };
            var res = SectionService.Update(section, GetUser());
            if (res == AccessCode.Succsess)
                return RedirectToAction("Index", new { id = sectionRequest.Id });
            else return Redirect(res);
        }

        public IActionResult Control(int id)
        {
            var res = SectionService.Read(id, GetUser());
            if (res.Item1 == AccessCode.Succsess && res.Item2.Access.CanUpdate)
                return View(new SectionControl(id, res.Item2));
            else return Redirect(res.Item1);
        }

        public IActionResult Add(int id)
        {
            if (SectionService.CanCreate(id, GetUser()))
                return View("Control", new SectionControl(id, null));
            else return Redirect(AccessCode.NoPremision);
        }

    }
}