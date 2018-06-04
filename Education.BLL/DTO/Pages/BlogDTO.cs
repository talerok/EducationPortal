using System;
using System.Collections.Generic;
using System.Text;

namespace Education.BLL.DTO.Pages
{
    public class BlogDTO
    {
        public int Page { get; set; }
        public int Pages { get; set; }
        public IEnumerable<NoteDTO> Notes { get; set; }
        public bool CanCreate { get; set; }
    }
}
