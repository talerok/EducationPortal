using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Forum
{
    public class SectionDTO
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public bool Open { get; set; }
        public string Name { get; set; }
        public AccessDTO Access { get; set; }
        public IEnumerable<ThemePreviewDTO> Themes { get; set; }
    }
}
