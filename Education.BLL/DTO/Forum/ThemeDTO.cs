using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Forum
{
    public class ThemeRoute
    {
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public SectionRoute SectionRoute { get; set; }
    }

    public class ThemeDTO
    {
        public int Id { get; set; }
        public bool Open { get; set; }
        public string Name { get; set; }
        public FullAccessDTO Access { get; set; }
        public int Pages { get; set; }
        public int CurPage { get; set; }
        public IEnumerable<MessageDTO> Messages { get; set; }
        public ThemeRoute Route { get; set; }
    }
}
