using Education.BLL.DTO.Forum;
using Education.DAL.Entities;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Education.Components
{
    public class UserGroupViewComponent
    {
        private const string reqButton = @"<input type=""hidden"" name=""Status"" value=""1""/><input type=""submit"" value=""Добавить""/>";
        private const string chButton = @"<input type=""submit"" value=""Обновить данные""/>";

        private string GetInfo(UserGroupDTO userGroupDTO)
        {
            return @"<div class=""Info""><div class=""UserName"">" 
                + userGroupDTO.UserInfo.Name 
                + @"</div><img class=""Avatar"" src=""" + userGroupDTO.UserInfo.AvatarPath + @"""/></div>";
        }

        private string GetEdit(string inside)
        {
            return @"<div class=""Edit""><form method = ""post"" action = ""/Group/ControlUser"" >" + inside + "</form></div>";
        }

        private string GetUserFormInfo(UserGroupDTO userGroupDTO, int GroupId)
        {
            return @"<input type=""hidden"" name=""GroupId"" value=""" + GroupId + @"""/><input type = ""hidden"" name =""UserID"" value =""" + userGroupDTO.UserInfo.Id + @"""/>";
        }

        private string GetNotInGroup(UserGroupDTO DTO, int GroupId)
        {
            return GetInfo(DTO) + GetEdit(GetUserFormInfo(DTO, GroupId) + reqButton);
        }

        private string GetOption(UserGroupStatus id, int vid, string name, UserGroupStatus sel)
        {
            var res = "<option value=\"" + vid + "\"";
            if (id != sel) return res + ">" + name + "</option>";
            else return res + " selected>" + name + "</option>";
        }

        private string GetSelect(UserGroupDTO user)
        {
            return @"<select name=""Status"">" + GetOption(UserGroupStatus.request, 0, "Запрос", user.Status)
            + GetOption(UserGroupStatus.member, 1, "Участник", user.Status)
            + GetOption(UserGroupStatus.owner, 2, "Модератор", user.Status)
            + @"<option value=""4"">Удалить</option></select>";
        }

        private string GetInGroup(UserGroupDTO DTO, int GroupId)
        {
            return GetInfo(DTO) + GetEdit(GetUserFormInfo(DTO, GroupId) + GetSelect(DTO) + chButton);
        }

        public HtmlString Invoke(UserGroupDTO DTO, int GroupId, bool inGroup)
        {
            if (inGroup) return new HtmlString(GetInGroup(DTO, GroupId));
            else return new HtmlString(GetNotInGroup(DTO, GroupId));
        }
    }
}
