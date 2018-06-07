using Education.BLL.DTO.Pages;
using Education.DAL.Entities.Pages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.Logic.Interfaces
{
    public interface IPageMap
    {
        IEnumerable<PageInfo> Get { get; }
        void Add(Page page);
        void Update(Page page);
        void Delete(int id);
    }
}
