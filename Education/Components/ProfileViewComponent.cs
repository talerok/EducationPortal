using Education.BLL.DTO.User;
using Education.BLL.Services.UserServices.Interfaces;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Education.Components
{
    public class ProfileViewComponent : ViewComponent
    {
        IClaimService ClaimService;
        public ProfileViewComponent(IClaimService claimService)
        {
            ClaimService = claimService;
        }

        private UserDTO GetUser()
        {
            return ClaimService.GetUser(HttpContext.User.Claims);
        }

        public HtmlString Invoke()
        {
            var user = GetUser();
            if(user == null)
                return new HtmlString("<div class=\"Login\"><a href = \"/Account/Login\">Войти</a><a href = \"/Account/Login\">Зарегестрироваться</a></div>");
            else
                return new HtmlString("<div class=\"Login\"><text>" + user.FullName + "</text><a href = \"/Profile/\">Профиль</a><a href=\"/Account/Logout\">Выйти</a></div>");
        }
    }
}
