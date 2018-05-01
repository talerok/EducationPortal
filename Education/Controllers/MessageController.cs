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
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Education.Controllers
{
    public class MessageController : DefaultController
    {
        private IMessageService MessageService;
        private IClaimService ClaimService;

        private UserDTO GetUser()
        {
            return ClaimService.GetUser(User.Claims);
        }

        public MessageController(IMessageService messageService, IClaimService claimService)
        {
            MessageService = messageService;
            ClaimService = claimService;
        }

        public IActionResult Index(int id) // Read
        {
            var res = MessageService.Read(id, GetUser());
            if (res.Item1 == AccessCode.Succsess) return View(res.Item2);
            else return Redirect(res.Item1);
        }

        [HttpPost]
        public IActionResult Create(int themeId, string text)
        {
            var messageDTO = new MessageEditDTO
            {
                ThemeId = themeId,
                Text = text
            };
            var res = MessageService.Create(messageDTO, GetUser());
            if (res.Code == AccessCode.Succsess)
                return RedirectToAction("Index", "Theme", new { id = themeId });
            else return Redirect(res.Code);
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            var res = MessageService.Delete(id, GetUser());
            return Redirect(res);
        }

        [HttpPost]
        public IActionResult Update(int id, string text)
        {
            var messageDTO = new MessageEditDTO
            {
                Id = id,
                Text = text
            };
            var user = GetUser();
            var res = MessageService.Update(messageDTO, user);
            if (res == AccessCode.Succsess)
            {
                var message = MessageService.Read(id, user);
                if (message.Item1 == AccessCode.Succsess)
                    return RedirectToAction("Index", "Theme", new { id = message.Item2.Route.ThemeId });      
            }
            return Redirect(res);
        }

        public IActionResult Control(int id)
        {
            var res = MessageService.Read(id, GetUser());
            if(res.Item1 == AccessCode.Succsess && res.Item2.Access.CanUpdate)
                return View(res.Item2);

            return Redirect(res.Item1);
        }


    }
}
