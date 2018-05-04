using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Pages
{
    class PageDTO : PageEditDTO
    {
        public AccessDTO Access { get; set; }
        public IEnumerable<PageDTO> ChildPages { get; set; }
    }
}
