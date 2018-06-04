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
using Microsoft.AspNetCore.Mvc;

namespace Education.Controllers
{
    public class BlogController : DefaultController
    {
        private IBlogService BlogService;
        private IClaimService ClaimService;

        public BlogController(IBlogService blogService, IClaimService claimService)
        {
            BlogService = blogService;
            ClaimService = claimService;
        }

        private UserDTO GetUser()
        {
            return ClaimService.GetUser(User.Claims);
        }

        public IActionResult Page(int id = 1)
        {
            var notes = BlogService.Get(GetUser(), id);
            return View(notes);
        }

        public IActionResult Control(int id = -1)
        {
            var user = GetUser();
            if (id == -1)
            {
                if (BlogService.CanCreate(user))
                    return View(null);
                else return Redirect(AccessCode.NoPremision);
            }

            var note = BlogService.Get(id, user);
            if (note.Item1 == AccessCode.Succsess)
                return View(note.Item2);
            else return Redirect(note.Item1);
        }

        public IActionResult Index(int id)
        {
            var res = BlogService.Get(id, GetUser());
            if(res.Item1 == AccessCode.Succsess)
                return View(res.Item2);
            return Redirect(res.Item1);
        }

        [HttpPost]
        public IActionResult Create(NoteEditDTO noteEditDTO)
        {
            var res = BlogService.Create(noteEditDTO, GetUser());
            if (res.Code == AccessCode.Succsess) return RedirectToAction("Index", new { res.Id });
            return Redirect(res.Code);
        }

        [HttpPost]
        public IActionResult Update(NoteEditDTO noteEditDTO)
        {
            var res = BlogService.Update(noteEditDTO, GetUser());
            if (res == AccessCode.Succsess) return RedirectToAction("Index", new { noteEditDTO.Id });
            return Redirect(res);
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            var res = BlogService.Delete(id, GetUser());
            if (res == AccessCode.Succsess) return RedirectToAction("Page");
            return Redirect(res);
        }
    }
}