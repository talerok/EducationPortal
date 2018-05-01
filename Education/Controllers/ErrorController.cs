using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Education.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View("Index","Ошибка доступа к данным");
        }

        public IActionResult NoPremision()
        {
            return View("Index", "Ошибка: отказано в доступе");
        }

        public IActionResult NoResult()
        {
            return View("Index", "Ошибка: запись не найдена");
        }


    }
}