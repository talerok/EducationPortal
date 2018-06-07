using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Education.BLL.DTO.User;
using Education.BLL.Services.AdminService.Interfaces;
using Education.BLL.Services.UserServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Education.Controllers
{
    public class AdminController : Controller
    {
        private IClaimService ClaimService;
        private IAdminService adminService;
        private UserDTO GetUser()
        {
            return ClaimService.GetUser(User.Claims);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetUsers(string name)
        {
            return View();
        }

    }
}