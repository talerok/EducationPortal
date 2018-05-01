using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Education.BLL.DTO.User;
using Education.BLL.Services.ForumServices.Interfaces;
using Education.BLL.Services.UserServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Education.Controllers
{
    public class HomeController : Controller
    {
        private IForumService ForumService;
        private IClaimService ClaimService;
        public HomeController(IForumService forumService, IClaimService claimService)
        {
            ForumService = forumService;
            ClaimService = claimService;
        }

        private UserDTO GetUser()
        {
            return ClaimService.GetUser(User.Claims);
        }

        public IActionResult Index()
        {
            return View(ForumService.Get(GetUser()));
        }


    }
}
