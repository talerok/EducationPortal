﻿using Education.BLL.DTO.Forum;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Education.Components
{
    public class NavigationViewComponent
    {
        private const string sep = @">>";

        private string MainPageNavigation()
        {
            return @"<a href=""/"">Главная страница</a>";
        }

        private string GroupNavigation(int id, string name)
        {
            return MainPageNavigation() + sep + @"<a href=""/Group/Index/" + id + @""">Группа " + name + "</a>";
        }

        private string SectionNavigation(int id, string name, SectionRoute route)
        {
            return GroupNavigation(route.GroupId, route.GroupName)
                + sep + @"<a href = ""/Section/Index/" + id + @""">Секция " + name + "</a>";
        }

        private string ThemeNavigation(int id, string name, ThemeRoute route)
        {
            return SectionNavigation(route.SectionId, route.SectionName, route.SectionRoute)
                + sep + @"<a href=""/Theme/Index/"+ id + @""">Тема " + name + "</a>";
        }

        public HtmlString Invoke(Object DTO)
        {
            if (DTO is GroupDTO)
            {
                var _DTO = DTO as GroupDTO;
                return new HtmlString(GroupNavigation(_DTO.Id, _DTO.Name));
            }
            else if (DTO is SectionDTO)
            {
                var _DTO = DTO as SectionDTO;
                return new HtmlString(SectionNavigation(_DTO.Id, _DTO.Name, _DTO.Route));
            }
            else if (DTO is ThemeDTO)
            {
                var _DTO = DTO as ThemeDTO;
                return new HtmlString(ThemeNavigation(_DTO.Id, _DTO.Name, _DTO.Route));
            }
            else return new HtmlString("");
        }

       
    }
}
