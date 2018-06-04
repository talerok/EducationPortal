using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Pages
{
    public class NoteEditDTO
    {
        public int Id { get; set; }
        public string Preview { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public bool Published { get; set; }
    }
}
