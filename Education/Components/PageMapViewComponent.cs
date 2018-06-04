using Education.BLL.DTO.Pages;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Education.Components
{
    public class PageMapViewComponent
    {
        private string Offset(int level)
        {
            return new string('-', level * 3);
        }

        private string Generate(PageInfo pageInfo, int SelectedId, int Id, int level = 0)
        {
            if (pageInfo.Id == Id) return "";

            string res = "<option ";
            if (SelectedId == pageInfo.Id)
                res += "selected ";
            res+= "value=\"" + pageInfo.Id + "\">" + Offset(level) + pageInfo.Name + "</option>";

            foreach (var page in pageInfo.Childs)
                res += Generate(page, SelectedId, Id, level + 1);
            return res;
        }


        public HtmlString Invoke(IEnumerable<PageInfo> map, string name, int SelectedId, int Id)
        {
            var res = "<select name=\"" + name + "\"><option value=\"-1\">Нет</option>";
            foreach(var page in map)
                res += Generate(page, SelectedId, Id, 0);
            return new HtmlString(res + "</select>");
        }
    }
}
