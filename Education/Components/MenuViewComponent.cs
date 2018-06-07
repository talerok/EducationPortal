using Education.BLL.Services.PageServices.Interfaces;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Education.Components
{
    public class MenuViewComponent
    {
        private IPageService PageService { get; set; }
        public MenuViewComponent(IPageService pageService)
        {
            PageService = pageService;
        }

        public HtmlString Invoke()
        {
            string res = "";
            foreach(var page in PageService.Map.Get)
                if(page.Published)
                    res += String.Format(@"<a href=""\Page\Index\{0}"">{1}</a>", page.Id, page.Name);
            return new HtmlString(res);
        }

    }

}
