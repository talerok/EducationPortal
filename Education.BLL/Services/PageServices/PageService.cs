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

namespace Education.BLL.Services.PageServices
{
    class PageService : IPageService
    {
        private IUOWFactory DataFactory;
        private IPageRules PageRules;
        private IDTOHelper DTOHelper;
        private IGetUserDTO GetUserService;

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
                childPages.Add(GetPreviewDTO(cp));
            return new PageDTO
            {
                ChildPages = childPages,
                Id = page.Id,
                Name = page.Name,
                ParentId = page.ParentPage != null ? page.ParentPage.Id : -1,
                Published = page.Published,
                Text = page.Text,
                Access = new DTO.AccessDTO
                {
                    CanDelete = PageRules.CanDelete(user, page),
                    CanRead = PageRules.CanRead(user, page),
                    CanUpdate = PageRules.CanEdit(user, page)
                }
            };
        }

        public bool CanCreate(UserDTO userDTO)
        {
            using(var Data = DataFactory.Get())
                return PageRules.CanCreate(GetUserService.Get(userDTO, Data));
        }

        public PagesDTO Get(UserDTO userDTO)
        {
            using(var Data = DataFactory.Get())
            {
                var user = GetUserService.Get(userDTO, Data);
                var pages = Data.PageRepository.Get()
                    .Where(x => x.ParentPage == null && PageRules.CanRead(user, x))
                    .Select(x => GetDTO(x, user));
                return new PagesDTO { CanCreate = PageRules.CanCreate(user), MainPages = pages };
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
                EditPage(page, pageEditDTO, Data);
                Data.PageRepository.Edited(page);
                Data.SaveChanges();
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
                Data.SaveChanges();
                return new CreateResultDTO(page.Id, AccessCode.Succsess);
            }
        }
    }
}
