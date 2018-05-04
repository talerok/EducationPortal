using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Components
{
    public class PagingViewComponent
    {
        private static int PagesCount = 3;

        private string GetRef(string url, string name)
        {
            return "<a href=\"" + url + "\">" + name + "</a>";
        }

        private string GetSelectedRef(string url, string name)
        {
            return "<a class=\"Selected\" href=\"" + url + "\">" + name + "</a>";
        }

        public HtmlString Invoke(string url, int page, int pages)
        {
            var res = new StringBuilder();

            res.Append("<div class=\"Paging\">");

            if (page != 1)
                res.Append(GetRef(url, "<<Первая"));
            
            int startpage = page <= PagesCount ? 1 : page - PagesCount;
            int endpage = pages - page <= PagesCount ? pages : pages + PagesCount;

            for(int i = startpage; i<=endpage; i++)
            {
                if (i == page) res.Append(GetSelectedRef(url + i, i.ToString()));
                else res.Append(GetRef(url + i, i.ToString()));
            }

            if (page != pages)
                res.Append(GetRef(url + pages, "Последняя>>"));

            return new HtmlString(res.ToString());
        }
    }
}
