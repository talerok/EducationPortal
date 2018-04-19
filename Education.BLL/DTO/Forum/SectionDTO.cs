using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Forum
{
    class SectionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ThemePreviewDTO> Themes { get; set; }
    }
}
