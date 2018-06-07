using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Pages
{
    public class PageInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<PageInfo> Childs { get; set; }
        public PageInfo Parent { get; set; }
        public bool Published { get; set; }
    }
}
