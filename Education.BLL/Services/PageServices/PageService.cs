using Education.BLL.DTO.Forum;
using Education.BLL.DTO.Pages;
using Education.BLL.DTO.User;
using Education.BLL.Logic.Interfaces;
using Education.DAL.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Education.DAL.Entities.Pages;
using Education.DAL.Entities;
using Education.BLL.Services.PageServices.Interfaces;
using Education.BLL.Logic;

namespace Education.BLL.Services.PageServices
{
    public class PageService : IPageService
    {
        private IUOWFactory DataFactory;
        private IPageRules PageRules;
        private IDTOHelper DTOHelper;
        private IGetUserDTO GetUserService;

        public IPageMap Map { get; private set; }

        private void InitMap()
        {
            using (var Data = DataFactory.Get())
                Map = new PageMap(Data.PageRepository.Get());
        }

        public PageService(IPageRules pageRules, IUOWFactory dataFactory, IGetUserDTO getUserService, IDTOHelper dtoHelper)
        {
            PageRules = pageRules;
            DataFactory = dataFactory;
            GetUserService = getUserService;
            DTOHelper = dtoHelper;
            //---------------------------
            InitMap();
        }

        private PagePreviewDTO GetPreviewDTO(Page page)
        {
            return new PagePreviewDTO { Id = page.Id, Name = page.Name };
        }

        private void EditPage(Page page, PageEditDTO pageEditDTO, IUOW Data)
        {
            if (page == null || pageEditDTO == null) return;
            page.Name = pageEditDTO.Name;
            if (pageEditDTO.ParentId >= 0)
                page.ParentPage = Data.PageRepository.Get().FirstOrDefault(x => x.Id == pageEditDTO.ParentId);
            else page.ParentPage = null;
            page.Published = pageEditDTO.Published;
            page.Text = pageEditDTO.Text;
        }

        private PageDTO GetDTO(Page page, User user)
        {
            var childPages = new List<PagePreviewDTO>();
            foreach (var cp in page.ChildPages)
                if(PageRules.CanRead(user, cp))
                    childPages.Add(GetPreviewDTO(cp));

            var res = new PageDTO
            {
                ChildPages = childPages,
                Id = page.Id,
                Name = page.Name,
                Published = page.Published,
                Text = page.Text,
                Access = new DTO.AccessDTO
                {
                    CanDelete = PageRules.CanDelete(user, page),
                    CanRead = PageRules.CanRead(user, page),
                    CanUpdate = PageRules.CanEdit(user, page)
                }
            };

            if (page.ParentPage != null)
            {
                res.ParentId = page.ParentPage.Id;
                res.ParentName = page.ParentPage.Name;
            }
            else res.ParentId = -1;

            return res;
        }

        private IEnumerable<PageInfo> GetMap(Page page)
        {
            var res = new List<PageInfo>();
            foreach (var ch in page.ChildPages)
            {
                res.Add(new PageInfo { Id = ch.Id, Name = ch.Name, Childs = GetMap(ch) });
            }
            return res;
        }

        private bool CheckCycle(Page parent, Page child)
        {
            if (parent == child) return false;
            Page curPage = parent.ParentPage;
            while (true)
            {
                if (curPage == null) return false;
                if (curPage == parent) return false; // Чтобы не зацикл. 
                if (curPage == child) return true;
                curPage = curPage.ParentPage;
            }
            
        }

        public (AccessCode,PageDTO) Get(int id, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var page = Data.PageRepository.Get().FirstOrDefault(X => X.Id == id);
                var user = GetUserService.Get(userDTO, Data);
                if (page == null) return (AccessCode.NotFound, null);
                if (!PageRules.CanRead(user, page)) return (AccessCode.NoPremision, null);
                return (AccessCode.Succsess,GetDTO(page, user));
            }
        }

        public AccessCode Update(PageEditDTO pageEditDTO, UserDTO userDTO)
        {
            using(var Data = DataFactory.Get())
            {
                var page = Data.PageRepository.Get().FirstOrDefault(X => X.Id == pageEditDTO.Id);
                var user = GetUserService.Get(userDTO, Data);
                if (page == null) return AccessCode.NotFound;
                if (!PageRules.CanEdit(user, page)) return AccessCode.NoPremision;
                //-------------------------
                if(pageEditDTO.ParentId > 0) // Проверка на цикличность
                {
                    var parent = Data.PageRepository.Get().FirstOrDefault(x => x.Id == pageEditDTO.ParentId);
                    if (parent != null && CheckCycle(parent, page)) return AccessCode.Error;
                }
                //-------------------------
                EditPage(page, pageEditDTO, Data);
                Data.PageRepository.Edited(page);
                Data.SaveChanges();
                Map.Update(page);
                return AccessCode.Succsess;
            }
        }

        public AccessCode Delete(int id, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var page = Data.PageRepository.Get().FirstOrDefault(X => X.Id == id);
                var user = GetUserService.Get(userDTO, Data);
                if (page == null) return AccessCode.NotFound;
                if (!PageRules.CanDelete(user, page)) return AccessCode.NoPremision;
                Data.PageRepository.Delete(page);
                Data.SaveChanges();
                Map.Delete(id);
                return AccessCode.Succsess;
            }
        }

        public CreateResultDTO Create(PageEditDTO pageEditDTO, UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
            {
                var user = GetUserService.Get(userDTO, Data);
                if (!PageRules.CanCreate(user)) return CreateResultDTO.NoPremision;
                var page = new Page();
                EditPage(page, pageEditDTO, Data);
                Data.PageRepository.Add(page);
                //Добавить проверку на цикл
                Data.SaveChanges();
                Map.Add(page);
                return new CreateResultDTO(page.Id, AccessCode.Succsess);
            }
        }

        public bool CanCreate(UserDTO userDTO)
        {
            using (var Data = DataFactory.Get())
                return PageRules.CanCreate(GetUserService.Get(userDTO, Data));
        }
    }
}
