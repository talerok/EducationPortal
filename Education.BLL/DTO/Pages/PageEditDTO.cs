using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Pages
{
    public class PageEditDTO : PagePreviewDTO
    {
        public int? ParentId { get; set; }
        public string Text { get; set; }
        public bool Published { get; set; }
    }
}
