using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Pages
{
    public class PageDTO : PageEditDTO
    {
        public AccessDTO Access { get; set; }
        public IEnumerable<PagePreviewDTO> ChildPages { get; set; }
        public string ParentName { get; set; }
    }
}
