using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Education.BLL.Services.UserServices.Interfaces;
namespace Education.Controllers
{
    public class InstallController : Controller
    {
        IInitDBService initDBService;
        public InstallController(IInitDBService ids)
        {
            initDBService = ids;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Init(string login, string password)
        {
            initDBService.InitAdmin(login, password);
            return View("Index","Home");
        }
    }
}