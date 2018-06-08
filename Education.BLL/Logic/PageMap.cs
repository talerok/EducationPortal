using Education.BLL.DTO.Pages;
using Education.BLL.Logic.Interfaces;
using Education.DAL.Entities.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Education.BLL.Logic
{
    public class PageMap : IPageMap
    {
        private Mutex UpdateMutex = new Mutex();

        public IEnumerable<PageInfo> Get { get; private set; } = new List<PageInfo>();

        private Dictionary<int, PageInfo> Pages = new Dictionary<int, PageInfo>();

        private void RemoveChild(PageInfo child)
        {
            if (child.Parent == null) return;
            (child.Parent.Childs as List<PageInfo>).Remove(child);
            child.Parent = null;
        }

        private void AddChild(int ParrentId, PageInfo child)
        {
            PageInfo parent = null;
            if (!Pages.TryGetValue(ParrentId, out parent)) return;
            (parent.Childs as List<PageInfo>).Add(child);
            child.Parent = parent;
        }

        public PageMap(IEnumerable<Page> pages)
        {
            Get = GenerateMap(pages);
        }

        private PageInfo GetPageInfo(Page page)
        {
            return new PageInfo { Id = page.Id, Name = page.Name, Published = page.Published, Childs = new List<PageInfo>() };
        }

        public void Add(Page page)
        {
            UpdateMutex.WaitOne();
            var PageInfo = GetPageInfo(page);
            if (page.ParentPage != null) {
                AddChild(page.ParentPage.Id, PageInfo);
            } else
                (Get as List<PageInfo>).Add(PageInfo);
            Pages.Add(page.Id, PageInfo);
            UpdateMutex.ReleaseMutex();
        }

        public void Update(Page page)
        {
            UpdateMutex.WaitOne();
            PageInfo PageInfo;
            if (!Pages.TryGetValue(page.Id, out PageInfo))
            {
                UpdateMutex.ReleaseMutex();
                return;
            }
            PageInfo.Name = page.Name;
            PageInfo.Published = page.Published;
            if(PageInfo.Parent == null)
            {
                if (page.ParentPage != null)
                {
                    (Get as List<PageInfo>).Remove(PageInfo);
                    AddChild(page.ParentPage.Id, PageInfo);
                }
            }
            else
            {
                if(page.ParentPage == null)
                {
                    RemoveChild(PageInfo);
                    (Get as List<PageInfo>).Add(PageInfo);
                }
                else if(page.ParentPage.Id != PageInfo.Parent.Id)
                {
                    RemoveChild(PageInfo);
                    AddChild(page.ParentPage.Id, PageInfo);
                }
            }
            UpdateMutex.ReleaseMutex();
        }

        public void Delete(int id)
        {
            UpdateMutex.WaitOne();
            PageInfo PageInfo;
            if (!Pages.TryGetValue(id, out PageInfo)) return;

            if (PageInfo.Parent != null)
                RemoveChild(PageInfo);
            else
                (Get as List<PageInfo>).Remove(PageInfo);
            Pages.Remove(id);
            
            UpdateMutex.ReleaseMutex();
        }

        private PageInfo GenerateElement(Page page)
        {
            var elem = GetPageInfo(page);
            var childs = elem.Childs as List<PageInfo>;
            Pages.Add(page.Id, elem);
            foreach (var ch in page.ChildPages)
            {
                var child = GenerateElement(ch);
                child.Parent = elem;
                childs.Add(child);
            }
            return elem;
        }

        private IEnumerable<PageInfo> GenerateMap(IEnumerable<Page> pages)
        {
            Pages.Clear();
            var res = new List<PageInfo>();
            var mainPages = pages.Where(x => x.ParentPage == null);
            foreach (var page in mainPages)
                res.Add(GenerateElement(page));
            return res;
        }


    }
}
