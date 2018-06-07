using System;
using System.Collections.Generic;
using System.Web;
using System.Security.Claims;
using System.Threading.Tasks;
using Education.BLL.DTO.User;
using Education.BLL.DTO;
using Education.BLL.Services.UserServices.Interfaces;
using Education.Models;
using Education.Captcha;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Education.Controllers
{
    [Produces("application/json")]
    public class AuthApiController : Controller
    {
        private IUserService userService;

        public AuthApiController(IUserService service)
        {
            userService = service;
        }

        private UserDTO GetUser()
        {
            return userService.ClaimService.GetUser(User.Claims);
        }

        #region Auth

        private async Task<IActionResult> ClearCookie()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [Route("api/Logout")]
        public async Task<IActionResult> Logout()
        {
            userService.Logout(User.Claims);
            return await ClearCookie();
        }

        [Authorize]
        [Route("api/ResetClaims")]
        public async Task<IActionResult> ResetClaims()
        {
            userService.ResetClaims(GetUser());
            return await ClearCookie();
        }

        [Route("api/Auth")]
        [HttpPost]
        public async Task<IActionResult> Auth(AuthRequest request)
        {
            
            AuthResult res = userService.Login(
                new LoginInfoDTO {
                    Login = request.Login,
                    Password = request.Password,
                    Key = request.SecretKey,
                    Browser = Request.Headers["User-Agent"],
                    IP = Request.HttpContext.Connection.RemoteIpAddress.ToString()
                }
           );
            if (res.Status == AuthStatus.Succsess)
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(res.Identity));
            return new JsonResult(
                new AuthResponse { AuthType = res.authType, KeyTime = res.KeyTime, Status = res.Status });

        }

        [Route("api/Register")]
        [HttpPost]
        [ValidateRecaptcha]
        public IActionResult Register(RegRequest regRequest)
        {
            var userDTO = new RegUserInfo
            {
                Email = regRequest.Email,
                Phone = regRequest.PhoneNumber,
                Name = regRequest.FullName,
                Login = regRequest.Login,
                Password = regRequest.Password
            };
            return new JsonResult(new RegResponse { Status = userService.Register(userDTO) });
        }
        #endregion
    }
}