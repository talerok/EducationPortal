using Education.BLL.DTO.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Education.Models
{
    public class SectionControl
    {
        public int GroupId { get; set; }
        public SectionDTO SectionDTO { get; set; }

        public SectionControl(int groupId, SectionDTO sectionDTO)
        {
            GroupId = groupId;
            SectionDTO = sectionDTO;
        }
    }
}
