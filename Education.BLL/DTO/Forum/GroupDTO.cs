using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Forum
{
    public class GroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public bool Open { get; set; }
        public GroupAccessDTO Access { get; set; }
        public IEnumerable<SectionDTO> Sections { get; set; }
    }
}
