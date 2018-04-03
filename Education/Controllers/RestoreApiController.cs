using System;
using System.Collections.Generic;
using System.Linq;
using Education.Captcha;
using Education.BLL.Services.UserServices.Interfaces;
using Education.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Education.Controllers
{
    [Produces("application/json")]
    public class RestoreApiController : Controller
    {
        IRestorePasswordService RestorePasswordService;
        public RestoreApiController(IRestorePasswordService restorePasswordService)
        {
            RestorePasswordService = restorePasswordService;
        }

        [HttpPost]
        [ValidateRecaptcha]
        [Route("api/Restore")]
        public IActionResult Restore(RestoreRequest request)
        {
            var result = RestorePasswordService.Restore(request.Login, request.Password, request.Key);
            return new JsonResult(result);
        }

    }
}