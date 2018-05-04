using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Forum
{
    public class SectionRoute
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
    }

    public class SectionDTO
    {
        public int Id { get; set; }
        public bool Open { get; set; }
        public string Name { get; set; }
        public FullAccessDTO Access { get; set; }
        public IEnumerable<ThemePreviewDTO> Themes { get; set; }
        public SectionRoute Route { get; set; }
    }
}
