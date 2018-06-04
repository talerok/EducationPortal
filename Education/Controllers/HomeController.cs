using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Education.BLL.DTO.User;
using Education.BLL.Services.ForumServices.Interfaces;
using Education.BLL.Services.PageServices.Interfaces;
using Education.BLL.Services.UserServices.Interfaces;
using Education.Models;
using Microsoft.AspNetCore.Mvc;

namespace Education.Controllers
{
    public class HomeController : Controller
    {
        private IForumService ForumService;
        private IBlogService BlogService;

        private IClaimService ClaimService;
        public HomeController(IForumService forumService, IClaimService claimService, IBlogService blogService)
        {
            ForumService = forumService;
            BlogService = blogService;
            ClaimService = claimService;
        }

        private UserDTO GetUser()
        {
            return ClaimService.GetUser(User.Claims);
        }

        public IActionResult Index()
        {
            var user = GetUser();
            return View(new MainPage {
                BlogDTO = BlogService.Get(user,1),
                ForumDTO = ForumService.Get(user)
            } );
        }


    }
}
