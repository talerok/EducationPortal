using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Education.BLL.DTO.Forum;
using Microsoft.AspNetCore.Mvc;

namespace Education.Controllers.Base
{
    public class DefaultController : Controller
    {
        protected IActionResult Redirect(AccessCode code)
        {
            switch (code)
            {
                case AccessCode.Error:
                    return RedirectToAction("Index", "Error");
                case AccessCode.NoPremision:
                    return RedirectToAction("NoPremision", "Error");
                case AccessCode.NotFound:
                    return RedirectToAction("NoResult", "Error");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        protected IActionResult ErrorCode(AccessCode code)
        {
            switch (code)
            {
                case AccessCode.Error:
                    return BadRequest();
                case AccessCode.NoPremision:
                    return Unauthorized();
                case AccessCode.NotFound:
                    return NotFound();
                default:
                    return Ok();
            }
        }
    }
}