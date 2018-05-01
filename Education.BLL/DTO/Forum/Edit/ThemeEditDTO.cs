using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Forum.Edit
{
    public class ThemeEditDTO
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public string Name { get; set; }
        public bool Open { get; set; }
        public string Text { get; set; }
    }
}
