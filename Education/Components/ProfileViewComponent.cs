using Education.BLL.DTO.User;
using Education.BLL.Logic.Interfaces;
using Education.BLL.Services.UserServices.Interfaces;
using Education.DAL.Interfaces;
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
        private IClaimService ClaimService;
        private IGetUserDTO GetUserService;
        private IUOWFactory DataFactory { get; set; }
        public ProfileViewComponent(IClaimService claimService, IGetUserDTO getUserDTO, IUOWFactory dataFactory)
        {
            ClaimService = claimService;
            GetUserService = getUserDTO;
            DataFactory = dataFactory;
        }

        private UserDTO GetUser()
        {
            return ClaimService.GetUser(HttpContext.User.Claims);
        }

        private string GetUserName(UserDTO userDTO)
        {
            using(var Data = DataFactory.Get())
            {
                var user = GetUserService.Get(userDTO, Data);
                if (user == null) return "";
                else return user.Info.FullName;
            }
        }

        public HtmlString Invoke()
        {
            var user = GetUser();
            if(user == null)
                return new HtmlString("<div class=\"Login\"><a href = \"/Account/Login\">Войти</a><a href = \"/Account/Login\">Зарегестрироваться</a></div>");
            else
                return new HtmlString("<div class=\"Login\"><text>" + GetUserName(user) + "</text><a href = \"/Profile/\">Профиль</a><a href=\"/Account/Logout\">Выйти</a></div>");
        }
    }
}
