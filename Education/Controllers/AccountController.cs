using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Education.BLL.Services.UserServices.Interfaces;
using Education.BLL.DTO.User;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Education.BLL.Logic;
using Education.Models;

namespace Education.Controllers
{
    public class AccountController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Profile");
        }
       
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index");

            return View();
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index");

            return View();
        }

        public IActionResult Restore()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index");

            return View();
        }

        
        public IActionResult Logout()
        {
            return RedirectToAction("Logout","AuthApi");
        }
       
    }
}
