using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Pages
{
    public class PagesDTO
    {
        public bool CanCreate { get; set; }
        public IEnumerable<PageDTO> MainPages { get; set; } 
    }
}
