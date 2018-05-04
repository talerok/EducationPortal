using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Pages
{
    class PageEditDTO
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public bool Published { get; set; }
    }
}
