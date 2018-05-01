using Education.BLL.DTO.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Education.Models
{
    public class ThemeControl
    {
        public int SectionId { get; set; }
        public ThemeDTO ThemeDTO { get; set; }

        public ThemeControl(int sectionId, ThemeDTO themeDTO)
        {
            SectionId = sectionId;
            ThemeDTO = themeDTO;
        }
    }
}
